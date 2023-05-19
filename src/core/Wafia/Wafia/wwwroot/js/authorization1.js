'use strict';
import React from "react";
import ReactDOM from "react-dom";
import "../css/reset.css";
import "../css/leaflet.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup } from "react-leaflet";
import LogInWindowPopUpCreaterComponent from "./logInPopUp.js";

import * as MapFeature from "./mapFeatures.js";
import { PersonalAreaRedirectButton } from "./common.js";


class Authorization extends LogInWindowPopUpCreaterComponent {
    constructor(props) {
        super(props);
        this.state = {
            userRight: EUserRight.kGuest
        };
    }

    renderLogInButton() {
        if (this.state.userRight === EUserRight.kGuest) {
            return (
                <button
                    onClick={() => {
                        super.setState({ screenState: EScreenState.kLogIn });
                    }}
                    type="button"
                    style={styles.LogInButtonStyle}>
                    <p style={styles.ButtonTextStyle}>Log In</p>
                </button>)
        }

        return PersonalAreaRedirectButton();
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

                {this.renderLogInButton()}
            </header>
        );
    }

    renderMain() {
        return (
            <main style={styles.BodyStyle}>
                <button
                    style={styles.SearchButtonStyle}
                    onClick={async () => {
                        //if (this.state.userRight === EUserRight.kGuest) {
                        //   window.location.assign("https://www.youtube.com/watch?v=ywthKNqI7uI");
                        //}
                        //else {
                            window.location.assign(EHtmlPages.kSearchScreen);
                        //}
                    }}>
                    <p style={styles.ButtonTextStyle}>Search</p>
                </button>
                <section style={styles.InterctiveMapStyle}>
                    <MapContainer
                        center={[60.00732, 30.37289]}
                        zoom={13}
                        style={{ width: '59wh', height: '63vh' }}>
                          <TileLayer
                            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"/>
                        <Marker
                            position={[60.00732, 30.37289]}
                            icon={MapFeature.DefaultMarkerIcon()}>
                            <Popup>
                                {"Polytech :)"}
                            </Popup>
                        </Marker>
                        <MapFeature.FindLocation />
                    </MapContainer>
                </section>
                <div>
                    {super.render()}
                </div>
            </main>
        );
    }

    render() {
        (async () => {
            const responce = await fetch("/api/get_session_info", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            })

            const sessionInfo = await responce.json();
            console.log("user status " + sessionInfo.user_rights);

            if (this.state.userRight != sessionInfo.user_rights)
            {
                this.setState({userRight: sessionInfo.user_rights});
            }
        })();

        return (
        <div>
            { this.renderHeader() }
            { this.renderMain() }
        </div>
        );
    }
}

const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Authorization));
