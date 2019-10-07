import React from "react";
import { message, Table, Button } from "antd";
import Spinner from "./spinner";
import { getEvent, getGuests, uploadGuestsList } from "../api/events-api";

const columns = [
    {
        title: "Name",
        dataIndex: "name",
        key: "name",
    },
    {
        title: "Email",
        dataIndex: "email",
        key: "email",
    },
    {
        title: "Comment",
        dataIndex: "comment",
        key: "comment",
    },
]

export default class PageEvent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: true,
            isImporting: false,
            guests: [],
            event: null,
        }
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        const eventId = this.getEventId();
        Promise.all([getEvent(eventId), getGuests(eventId)])
            .then(([event, guests]) => {
                this.setState({ isLoading: false, event, guests });
            })
            .catch(err => {
                this.setState({ isLoading: false, guests: [] });
            });
    }

    handleFileSelected = e => {
        const eventId = this.getEventId();
        this.setState({ isImporting: true });
        uploadGuestsList(eventId, e.target.files[0])
            .then(result => {
                if (result.isSuccess) {
                    return Promise.all([Promise.resolve(result), getEvent(eventId), getGuests(eventId)]);
                }
                return Promise.all([Promise.resolve(result), Promise.resolve(null), Promise.resolve(null)]);
            })
            .then(([importResult, event, guests]) => {
                if (importResult.isSuccess) {
                    this.setState({ isImporting: false, event, guests });
                    message.success("Guests list has been imported successfully.");
                } else {
                    this.setState({ isImporting: false });
                    message.warning(importResult.message);
                }
                document.getElementById("guests-file").value = "";
            })
            .catch(err => {
                message.error("Error. Guests list has not been imported.")
                this.setState({ isImporting });
                document.getElementById("guests-file").value = "";
            });
    }

    getEventId() {
        return this.props.match.params.eventId;
    }

    getRemainingSpaces() {
        const event = this.state.event;
        const count = event ? event.maxGuests - event.invitedGuestsCount : 0;
        return count > 0 ? count : 0;
    }

    renderEventDetails() {
        if (!this.state.event) {
            return <p>Event data is not available</p>
        }

        const { name, type, eventDate, maxGuests } = this.state.event;
        return (
            <ul>
                <li>Event name: <b>{name}</b></li>
                <li>Type: <b>{type}</b></li>
                <li>Date: <b>{eventDate}</b></li>
                <li>Remaining spaces: <b>{this.getRemainingSpaces()}</b></li>
            </ul>
        )
    }

    render() {
        const { isLoading, isImporting, guests } = this.state;
        if (isLoading) {
            return <Spinner/>;
        }

        const eventId = this.getEventId();

        return (
            <>
                <h1>Manage guests</h1>
                <Button icon="arrow-left" href="#/">Back to list</Button>
                <div className="event-details">
                    {this.renderEventDetails()}
                </div>
                <div className="guests-list">
                    <h3>Guests list</h3>
                    <div className="guests-list__buttons">
                        <Button icon="download" href={`/api/events/${eventId}/guests-csv`}>Export</Button>
                        <div className="guests-list__import">
                            <b>Import: </b>&nbsp;&nbsp;&nbsp;
                            <input id="guests-file" className="guests-list__file" type="file" name="file" onChange={this.handleFileSelected} disabled={isImporting}/>
                            <span className="guests_list__import-status">
                                { isImporting ? "File is importing..." : "" }
                            </span>
                        </div>    
                    </div>
                    <Table columns={columns} dataSource={guests} rowKey="id"/>
                </div>    
            </>    
        );
    }
}