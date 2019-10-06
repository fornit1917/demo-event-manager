import React from "react";
import { Table, Button, message } from "antd";
import Spinner from "./spinner";
import { getEvents, markEventAsArchive } from "../api/events-api";

export default class PageEventList extends React.Component {
    constructor(props) {
        super(props);

        this.columns = [
            {
                title: "Event",
                dataIndex: "name",
                key: "name",
            },
            {
                title: "Place",
                dataIndex: "place",
                key: "place",
            },
            {
                title: "Type",
                dataIndex: "type",
                key: "type",
            },
            {
                title: "Max guests",
                dataIndex: "maxGuests",
                key: "maxGuests",
            },
            {
                title: "Date",
                dataIndex: "eventDate",
                key: "eventDate",
            },
            {
                title: "Action",
                key: "action",
                render: row => {
                    return (
                        <span>
                            <a href={`#/events/${row.id}`}>Manage guests</a>
                            <div className="ant-divider ant-divider-vertical" role="separator"></div>
                            <a href="#" onClick={this.handleArchiveClick} data-id={row.id}>Archive</a>
                        </span>    
                    );
                }
            },
        ];

        this.state = {
            isLoading: false,
            events: [],
        }

        this.isPending = false;
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        getEvents()
            .then(events => {
                this.setState({ isLoading: false, events });
            })
            .catch(() => {
                this.setState({ isLoading: false, events: [] })
            });
    }

    handleArchiveClick = e => {
        e.stopPropagation();
        e.preventDefault();
        if (this.isPending) {
            return;
        }

        if (confirm("Are you shure that you want to archive event?")) {
            this.isPending = true;
            markEventAsArchive(e.currentTarget.dataset.id)
                .then(() => {
                    return getEvents();
                })
                .then(events => {
                    this.isPending = false;
                    message.success("Event has been archived successfully");
                    this.setState({ events })
                });
        }
    }

    render() {
        const { isLoading, events } = this.state;
        if (isLoading) {
            return <Spinner />;
        }

        return (
            <div className="events-list">
                <h1>Active events list</h1>
                <div className="events-list__add-btn">
                    <Button type="primary" icon="plus" href="#/events/create">Add event</Button>
                </div>    
                <div className="events-list__table">
                    <Table columns={this.columns} dataSource={events} rowKey="id"/>
                </div>    
            </div>
        );
    }
}