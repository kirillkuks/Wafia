'use strict';
import React from "react";
import ReactDOM from "react-dom";
import "../css/reset.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup } from "react-leaflet";
import LogInWindowPopUpCreaterComponent from "./logInPopUp.js";


class PersonalArea extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};
    }

    render() {
        return(
        <div>
            {this.renderHeader()}
        </div>
        );
    }


    renderHeader() {
        return (
            <header style={styles.HeaderStyle}>
                <div style={styles.HeaderTitle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                </div>

                <a href={EHtmlPages.kGuestScreen}>
                    <img src="../img/backToGuestScreen.png"></img>
                </a>
            </header>
        );
    }
}



const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(PersonalArea));
