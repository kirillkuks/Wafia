'use strict';
import React, { useState } from "react";
import ReactDOM from "react-dom";
import { Dropdown, Row, Col } from "react-bootstrap";
import { Form } from "react-bootstrap";

import RangeSlider from 'react-bootstrap-range-slider';

import "../css/reset.css";
import "../css/leaflet.css";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/css/bootstrap.css"
import "react-bootstrap-range-slider/dist/react-bootstrap-range-slider.css"
import "../css/app.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import { MapContainer, TileLayer, useMap, Marker, Popup, Polygon } from "react-leaflet";

import * as MapFeature from "./mapFeatures.js";
import { PersonalAreaRedirectButton, InfrastructureElementPriority } from "./common.js";


const AllCities = [
    {
        name: "Saint-Petersburg",
        lat: 59.95001,
        lon: 30.31661
    },
    {
        name: "Moscow",
        lat: 55.75583,
        lon: 37.61778
    },
    {
        name: "Kaliningrad",
        lat: 54.71666,
        lon: 20.49991
    }
];

const AllCountries = [
    "Russia"
];

const AllElements = [
    "School",
    "Hospital",
    "Underground",
    "Mall",
    "Unifersity",
    "Church",
    "Pharmacy"
]


class Search extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userRight: null,
            userLogin: "",
            showMapOptions: false,
            activeCity: "",
            activeCountry: "",
            activeLat: 54.5920,
            activeLon: 22.2013,
            requireFlyTo: false,
            useMapOptions: false,
            area: [],
            drawArea: false,


            elementsPriority: Array.apply(null, Array(AllElements.length)).map(function () { return 0; })
        }
    }

    render() {
        (async () => {
            //const responce = await fetch("/api/get_session_info", {
            //    method: "POST",
            //    headers: { "Accept": "application/json", "Content-Type": "application/json" }
            //});


            //if (responce.ok) {
            //    const sessionInfo = await responce.json();
            //    console.log("user status " + sessionInfo.user_rights);

            //    if (this.state.userRight != sessionInfo.user_rights)
            //    {
            //        console.log(sessionInfo.user_login);
            //        this.setState({userRight: sessionInfo.user_rights, userLogin: sessionInfo.user_login, requireFlyTo: false});
            //    }
            //}
            //else {
            //    console.log("user status developer (debug only)");

            //    this.setState({userRight: EUserRight.kAdmin, userLogin: "developer", requireFlyTo: false});
            //}

            const cityResponse = await fetch("/api/get_cities", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            });

            if (cityResponse.ok) {
                const cityJson = await cityResponse.json();
                console.log(cityJson);
            }
        })();

        console.log("area: " + this.state.area[0]);

        return (
            <div>
                {this.renderHeader()}
                {this.renderMap()}
                {this.renderSearchParams()}
                {this.state.useMapOptions ? this.renderMapOptions() : null}
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
        console.log("active coords: " + this.state.activeLat + " | " + this.state.activeLon);

        return (
            <section style={styles.InterctiveMapStyle}>
                <MapContainer
                    center={[this.state.activeLat, this.state.activeLon]}
                    zoom={13}
                    style={{ width: '59wh', height: '63vh' }}>
                        <TileLayer
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"/>
                        {this.state.requireFlyTo ? <MapFeature.MoveTo lat={this.state.activeLat} lon={this.state.activeLon} /> :  null}
                        <MapFeature.ClickProcesser mapComp={this}/>
                        {this.state.drawArea ?
                            <Polygon pathOptions={{color: "purple"}} positions={this.state.area} /> : null}
                </MapContainer>
            </section>
        );
    }

    renderSearchParams() {
        return (
            <div style={styles.SearchParamsBackgroundStyle}>
                {this.renderAreaSearchParams()}
                {this.renderSpecificObjects()}
                {this.renderElementsManageParams()}
                {this.renderSearchManageButtons()}
            </div>
        );
    }

    renderAreaSearchParams() {
        return (
            <div>
                <h1 style={styles.AreaParamsHelperTextSyle}>Search Area</h1>
                {this.renderDropdown(
                    this.state.activeCountry === "" ? "Country" : this.state.activeCountry,
                    "2vh",
                    AllCountries.map(
                        (country) => (
                            <Dropdown.Item onClick={() => {
                                this.setState({activeCountry: country, requireFlyTo: false})
                            }}>
                                {country}
                            </Dropdown.Item>
                        )
                    ).concat(
                        [<Dropdown.Divider />,
                        <Dropdown.Item onClick={() => {
                            this.setState({activeCountry: "", requireFlyTo: false})
                        }}>
                            Reset
                        </Dropdown.Item>]
                    )
                )}
                {this.renderDropdown(
                    this.state.activeCity === "" ? "City" : this.state.activeCity,
                    "6vh",
                    AllCities.map(
                        (city) => (
                            <Dropdown.Item onClick={() => {
                                this.setState({
                                    activeCity: city.name,
                                    activeLat: city.lat,
                                    activeLon: city.lon,
                                    requireFlyTo: true
                                });
                            }}>
                                {city.name}
                            </Dropdown.Item>
                        )
                    ).concat(
                        [<Dropdown.Divider />,
                        <Dropdown.Item onClick={() => {
                            this.setState({activeCity: "", requireFlyTo: false})
                        }}>
                            Reset
                        </Dropdown.Item>]
                    )
                )}
            </div>
        );
    }

    renderSpecificObjects() {
        return (
            <div style={styles.SpecificObjectParamsStyle}>

            </div>
        )
    }

    renderElementsManageParams() {
        return (
            <div>
                <h1 style={styles.InfrastructureElementsHelperTextStyle}>Infrastructure elements</h1>
                <div style={styles.InfrastructureElementsListStyle}>
                    {this.renderElementParamPriority()}
                </div>
            </div>
        );
    }

    renderSearchManageButtons() {
        return (
            <div>
                <button
                    type="button"
                    style={styles.SearchManageSearchButton}
                    onClick={() => {
                        console.log(this.state.elementsPriority);
                    }}>
                    <p style={styles.ButtonTextStyle}>Search</p>
                </button>
                <button
                    type="button"
                    style={styles.SearchManageSaveButton}>
                    <p style={styles.ButtonTextStyle}>Save</p>
                </button>
                <button
                    type="button"
                    style={styles.SearchManageMapOptionsButton}
                    onClick={() => {
                        this.setState({useMapOptions: !this.state.useMapOptions, requireFlyTo: false})
                    }}>
                    <p style={styles.ButtonTextStyle}>Map options</p>
                </button>
            </div>
        );
    }

    renderMapOptions() {
        function coordArrayToString(arr) {
            let res = "";

            arr.forEach(element => {
                res += element[0].toFixed(2);
                res += ","
                res += element[1].toFixed(2);
                res += ";"
            });

            return res;
        }

        return (
        <div style={styles.MapOptionsStyle}>
            <input
                placeholder={coordArrayToString(this.state.area)}
                style={styles.MapOptionsPointsStyle}
                readOnly="readonly">
            </input>
            <button
                type="button"
                style={styles.MapOptionsSubmitAreaStyle}
                onClick={() => {
                    this.setState({drawArea: true, requireFlyTo: false})
                }}>
                Submit Area
            </button>
            <button
                type="button"
                style={styles.MapOptionsResetAreaStyle}
                onClick={() => {
                    this.setState({drawArea: false, area: [], requireFlyTo: false})
                }}>
                Reset Area
            </button>
        </div>
        );
    }

    renderDropdown(toggleString, top, items) {
        return (
            <div style={{position: "absolute", left: "2vw", top: top, width: "20vw", height: "4vh"}}>
                <Dropdown>
                    <Dropdown.Toggle
                        variant="success"
                        id="cityDropdown">
                        {toggleString}
                    </Dropdown.Toggle>

                    <Dropdown.Menu>
                        {items}
                    </Dropdown.Menu>
                </Dropdown>
            </div>
        );
    }

    renderElementParamPriority() {
        function calcHelperTextStyle(idx) {
            let topStr = (7 * idx + 1).toString() + "vh";

            return Object.assign({}, styles.InfrastructureElementNameStyle, {
                top: topStr
            });
        }

        function calcRangeStyle(idx) {
            let topStr = (7 * idx + 4).toString() + "vh";

            return Object.assign({}, styles.InfrastructureElementPriorityStyle, {
                top: topStr
            });
        }

        return (
            <div>
            {AllElements.map((element, idx) => (
                <div>
                    <h1 style={calcHelperTextStyle(idx)}>{element}</h1>
                    <div style={calcRangeStyle(idx)}>
                    <Form>
                        <Form.Group as={Row}>
                            <Col xs="9">
                                <RangeSlider
                                    value={this.state.elementsPriority[idx]}
                                    min={0}
                                    max={InfrastructureElementPriority.length - 1}
                                    onChange={e => this.setState({elementsPriority: this.state.elementsPriority.map((c, i) => {
                                        if (i === idx) {
                                            return Number(e.target.value);
                                        }
                                        else {
                                            return c;
                                        }
                                    }),
                                    requireFlyTo: false
                                })}
                                />
                            </Col>
                            <Col xs="3">
                                <Form.Control value={InfrastructureElementPriority[this.state.elementsPriority[idx]]} />
                            </Col>
                        </Form.Group>
                    </Form>
                    </div>
                </div>
            ))}
            </div>
        )
    }
}


const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Search));
