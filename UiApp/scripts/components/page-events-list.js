import React from "react";
import { Table, Button } from "antd";
import Spinner from "./spinner";
import { getEvents } from "../api/events-api";

const columns = [
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
    }
];

export default class PageEventList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: false,
            events: [],
        }
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
                    <Table columns={columns} dataSource={events} rowKey="id"/>
                </div>    
            </div>
        );
    }
}