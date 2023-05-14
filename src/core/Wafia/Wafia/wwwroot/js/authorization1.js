'use strict';
import React from "react";
import ReactDOM from "react-dom";
import "../css/reset.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup } from "react-leaflet";
import LogInWindowPopUpCreaterComponent from "./logInPopUp.js";


class Authorization extends LogInWindowPopUpCreaterComponent {
    constructor(props) {
        super(props);
        this.state = {
            userRight: EUserRight.kGuest
        };
    }

    logInButtonOutputString() {
        console.log(this.state.userRight);

        if (this.state.userRight === EUserRight.kGuest) {
            return <p style={styles.ButtonTextStyle}>Log In</p>
        } else {
            return <p style={styles.ButtonTextStyle}>Personal Area</p>
        }
    }

    logInButtonOnClick() {
        if (this.state.userRight === EUserRight.kGuest) {
            return () => {
                super.setState({ screenState: EScreenState.kLogIn })
            }
        }

        return () => {
            window.location.assign(EHtmlPages.kPersonalArea);
        }
    }

    renderLogInButton() {
        return <button
            onClick={this.logInButtonOnClick()}
            type="button"
            style={styles.LogInButtonStyle}>
            {this.logInButtonOutputString()}
        </button>
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
                        const response = await fetch("/api/search", {
                            method: "GET",
                            headers: { "Accept": "application/json" }
                        });

                        if (response.ok) {
                            window.location.assign("https://www.youtube.com/watch?v=ywthKNqI7uI");
                        }
                    }}>
                    <p style={styles.ButtonTextStyle}>Search</p>
                </button>
                <section style={styles.InterctiveMapStyle}>
                </section>
                {super.render()}
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

        // return (
        //     <MapContainer center={[51.505, -0.09]} zoom={13} scrollWheelZoom={false}>
        //         <TileLayer
        //             attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        //             url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        //         />
        //         <Marker position={[51.505, -0.09]}>
        //             <Popup>
        //             A pretty CSS3 popup. <br /> Easily customizable.
        //             </Popup>
        //         </Marker>
        //     </MapContainer>
        // );
    }
}

const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Authorization));
