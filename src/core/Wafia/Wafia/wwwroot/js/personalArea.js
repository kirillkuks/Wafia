'use strict';
import React from "react";
import ReactDOM from "react-dom";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/css/bootstrap.css"
import "react-bootstrap-range-slider/dist/react-bootstrap-range-slider.css"
import "../css/reset.css";
import "../css/app.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup } from "react-leaflet";
import LogInWindowPopUpCreaterComponent from "./logInPopUp.js";
import { Dropdown } from "react-bootstrap";


const UserLogOutToggle = React.forwardRef(({ children, onClick }, ref) => (
    <a
      href=""
      ref={ref}
      onClick={(e) => {
        e.preventDefault();
        onClick(e);
      }}
    >
      {children}
      &#x25bc;
    </a>
  ));


class PersonalArea extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userRight: null,
            userLogin: ""
        };
    }

    render() {
        (async () => {
            const responce = await fetch("/api/get_session_info", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            });

            const sessionInfo = await responce.json();
            console.log("user status " + sessionInfo.user_rights);

            if (this.state.userRight != sessionInfo.user_rights)
            {
                console.log(sessionInfo.user_login);
                this.setState({userRight: sessionInfo.user_rights, userLogin: sessionInfo.user_login});
            }
        })();

        return(
        <div>
            { this.renderHeader() }
            { this.renderMain() }
        </div>
        );
    }

    renderHeader() {
        return (
            <header style={styles.HeaderStyle}>
                <div style={styles.HeaderTitle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                </div>

                <a
                    href={EHtmlPages.kGuestScreen}
                    style={styles.BackToGuestScreenButtonStyle}>
                    <img src="../img/backToGuestScreen.png"></img>
                </a>

                <div style={styles.UserLoginDropdownStyle}>
                    <Dropdown>
                        <Dropdown.Toggle
                            as={UserLogOutToggle}
                            id="UserLogOut">
                            <p style={styles.UserLoginTextStyle}>{this.state.userLogin}</p>
                        </Dropdown.Toggle>
                        {this.checkLogOut()}
                        <Dropdown.Menu>
                            <Dropdown.Item onClick={async () => {
                                const response = await fetch("/api/logout", {
                                    method: "POST",
                                    headers: { "Accept": "application/json", "Content-Type": "application/json" }
                                });

                                const answer = await response.json();
                                window.location.assign(EHtmlPages.kGuestScreen);
                                }
                            }>Log Out</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </div>
            </header>
        );
    }

    checkLogOut() {
        console.log("render log out");
        return null;
    }

    renderMain() {
        return (
            <main>
                <img
                    src="../img/personalAreaBorders.png"
                    style={styles.PesonalAreaBordersStyle}>
                </img>
                {this.renderAdmin()}
                {this.renderHistoryField()}
            </main>
        );
    }

    renderAdmin() {
        if (this.state.userRight != EUserRight.kAdmin) {
            return null;
        }

        return ([
            <button
                key="UpdateDatabaseButton"
                style={styles.UpdateDatabaseButton}>
                <p style={styles.ButtonTextStyle}>Update Database</p>
            </button>,
            <button
                key="GiveRightsButton"
                style={styles.GiveRightsButton}>
                <p style={styles.ButtonTextStyle}>Give Rights</p>
            </button>
        ]);
    }

    renderHistoryField() {
        return (
            <div style={styles.HistoryFieldStyle}>
                <img src="../img/historyField.png"></img>
                <button
                   style={styles.DeleteFromHistoryButtonStyle}
                   onClick={() => console.log("Delete from history")}>
                   <img src="../img/deleteFromHistory.png"></img>
                </button>
            </div>
            
        );
    }
}


const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(PersonalArea));
