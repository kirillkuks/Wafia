'use strict';
import React from "react";
import ReactDOM from "react-dom";
import { Dropdown } from "react-bootstrap";

import "../css/reset.css";
import "../css/leaflet.css";
import "../css/bootstrap/bootstrap.min.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup } from "react-leaflet";

import { PersonalAreaRedirectButton } from "./common.js";


const AllCities = [
    "Saint-Petersburg",
    "Moscow",
    "Kaliningrad"
];


class Search extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userRight: null,
            userLogin: "",
            showMapOptions: false,
            activeCity: ""
        }
    }

    render() {
        (async () => {
            const responce = await fetch("/api/get_session_info", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            })

            if (responce.ok) {
                const sessionInfo = await responce.json();
                console.log("user status " + sessionInfo.user_rights);

                if (this.state.userRight != sessionInfo.user_rights)
                {
                    console.log(sessionInfo.user_login);
                    this.setState({userRight: sessionInfo.user_rights, userLogin: sessionInfo.user_login});
                }
            }
            else {
                console.log("user status developer (debug only)");
                this.setState({userRight: EUserRight.kAdmin, userLogin: "developer"});
            }
        })();

        return (
            <div>
                {this.renderHeader()}
                {this.renderMap()}
                {this.renderSearchParams()}
            </div>
        );
    }

    renderHeader() {
        return (
            <header style={styles.HeaderStyle}>
                <div style={styles.HeaderTitle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                </div>

                <a href={EHtmlPages.kAbout}>
                    <button style={styles.AboutButtonStyle}>
                        <p style={styles.ButtonTextStyle}>About</p>
                    </button>
                </a>

                {PersonalAreaRedirectButton()}
            </header>
        );
    }

    renderMap() {
        return (
            <section style={styles.InterctiveMapStyle}>
                <MapContainer
                    center={[54.5920, 22.2013]}
                    zoom={13}
                    style={{ width: '59wh', height: '63vh' }}>
                        <TileLayer
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"/>
                </MapContainer>
            </section>
        );
    }

    renderSearchParams() {
        return AllCities.length == 0 ? null : (
            <div style={styles.SearchParamsBackgroundStyle}>
                    <Dropdown>
                        <Dropdown.Toggle variant="success" id="cityDropdown">
                            {this.state.activeCity === "" ? "City" : this.state.activeCity}
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            {AllCities.map(
                                (city) => (
                                    <Dropdown.Item onClick={() => {
                                        this.setState({activeCity: city})
                                    }}>
                                        {city}
                                    </Dropdown.Item>
                                )
                            )}
                        </Dropdown.Menu>
                    </Dropdown>
            </div>
        );
    }
}


const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Search));
