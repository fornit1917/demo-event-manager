import React from "react";
import { message, Table, Button } from "antd";
import Spinner from "./spinner";
import { getEvent, getGuests } from "../api/events-api";

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
            guests: [],
            event: null,
        }
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        const eventId = this.getEventId();
        Promise.all([getEvent(eventId), getGuests(eventId)])
            .then(([event, guests]) => {
                console.log({event});
                console.log({guests});
                this.setState({ isLoading: false, event, guests });
            })
            .catch(err => {
                this.setState({ isLoading: false, guests: [] });
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
        const { isLoading, guests } = this.state;
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
                    <div class="guests-list__buttons">
                        <Button icon="download" href={`/api/events/${eventId}/guests-csv`}>Export</Button>
                    </div>
                    <Table columns={columns} dataSource={guests} rowKey="id"/>
                </div>    
            </>    
        );
    }
}