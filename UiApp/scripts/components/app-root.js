import React from "react";
import PropTypes from "prop-types";
import { Route, HashRouter, withRouter } from "react-router-dom";

import PageEvent from "./page-event";
import PageCreateEvent from "./page-create-event";
import PageEventsList from "./page-events-list";

export default class AppRoot extends React.Component {
    render() {
        return (
            <div className="app-content">
                <HashRouter>
                    <Route exact path="/" component={PageEventsList} />
                    <Route exact path="/events/create" component={PageCreateEvent} />
                    <Route exact path="/events/:eventId(\d+)" component={PageEvent} />
                </HashRouter>
            </div>    
        )
    }
}