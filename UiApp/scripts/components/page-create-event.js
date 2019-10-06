import React from "react";
import { Form, Input, Icon, Button, InputNumber, Select, message, DatePicker } from "antd";
import moment from "moment";
import { createEvent } from "../api/events-api";

export default class PageCreateEvent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            place: "",
            type: "Conference",
            eventDate: moment(),
            maxGuests: "",

            isPending: false,
            errors: {},
        };
    }

    handleTextValueChange = e => {
        const key = e.currentTarget.dataset.key;
        const value = e.currentTarget.value;

        const errors = !!this.state.errors[key] ? { ...this.state.errors, [key]: undefined } : this.state.errors;
        this.setState({ [key]: value, errors });
    }

    handleMaxGuestsChange = maxGuests => {
        if (typeof maxGuests == "string") {
            maxGuests = maxGuests.replace( /^\D+/g, '');
        }
        const errors = !!this.state.errors.maxGuests ? { ...this.state.errors, maxGuests: undefined } : this.state.errors;
        this.setState({ maxGuests, errors });
    }

    handleTypeChange = type => {
        this.setState({ type });
    }

    handleEventDateChange = eventDate => {
        this.setState({ eventDate });
    }

    handleSubmit = e => {
        const isValid = this.validate();
        if (isValid) {
            const { name, place, type, eventDate, maxGuests } = this.state;
            this.setState({ isPending: true });
            createEvent({ name, place, type, eventDate: eventDate.format("YYYY-MM-DD"), maxGuests })
                .then(data => {
                    message.success("Event has been saved successfully.");
                    setTimeout(() => location.replace("#/"), 1000);
                })
                .catch(() => {
                    message.error("Event has not been saved.");
                    this.setState({ isPending: false });
                });
        }
    }

    validate() {
        const errors = {};
        if (this.state.name === "") {
            errors.name = "Event name can not be empty";
        }
        if (this.state.place === "") {
            errors.place = "Place name can not be empty";
        }

        const maxGuestsNumber = Number(this.state.maxGuests);
        if (isNaN(maxGuestsNumber) || maxGuestsNumber <= 0) {
            errors.maxGuests = "Value of max guests should be a positive integer number";
        }

        this.setState({ errors });

        return Object.keys(errors).length == 0;
    }

    getValidateStatus(fieldKey) {
        return !!this.state.errors[fieldKey] ? "error" : "success";
    }

    render() {
        const { name, place, type, eventDate, maxGuests, errors, isPending } = this.state;
        return (
            <>
                <h1>Add new event</h1>
                <Button href="#/" icon="arrow-left">Back to list</Button>
                <Form className="event-form" onSubmit={this.handleSubmit}>
                    <Form.Item label="Event name" required validateStatus={this.getValidateStatus("name")} help={errors.name}>
                        <Input data-key="name" placeholder="Event name" value={name} onChange={this.handleTextValueChange} />
                    </Form.Item>

                    <Form.Item label="Place name" required validateStatus={this.getValidateStatus("place")} help={errors.place}>
                        <Input data-key="place" placeholder="Place name" value={place} onChange={this.handleTextValueChange} />
                    </Form.Item>

                    <Form.Item label="Event type" required>
                        <Select className="event-form__type" onChange={this.handleTypeChange} value={type}>
                            <Select.Option value="Conference">Conference</Select.Option>
                            <Select.Option value="Seminar">Seminar</Select.Option>
                            <Select.Option value="Hackaton">Hackaton</Select.Option>
                        </Select>
                    </Form.Item>

                    <Form.Item label="Event date" required validateStatus={this.getValidateStatus("eventDate")} help={errors.eventDate}>
                        <DatePicker onChange={this.handleEventDateChange} value={eventDate} disabledDate={disabledDate} allowClear={false}/>
                    </Form.Item>

                    <Form.Item label="Max guests" required validateStatus={this.getValidateStatus("maxGuests")} help={errors.maxGuests}>
                        <InputNumber data-key="maxGuests" min={1} value={maxGuests} onChange={this.handleMaxGuestsChange}/>
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary" htmlType="submit" icon="save" disabled={isPending}>Submit</Button>
                    </Form.Item>
                </Form>
            </>
        );
    }
}

function disabledDate(current) {
    return current && current < moment().endOf('day');
}