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


export const FilterMenu = React.forwardRef(
({ children, style, className, 'aria-labelledby': labeledBy }, ref) => {
    const [value, setValue] = useState("");

    const compareValue = value.toLocaleLowerCase();
  
    return (
    <div
        ref={ref}
        style={style}
        className={className}
        aria-labelledby={labeledBy}
    >
        <Form.Control
        autoFocus
        className="mx-3 my-2 w-auto"
        placeholder="Type to filter..."
        onChange={(e) => setValue(e.target.value)}
        value={value}
        />
        <ul className="list-unstyled">
        {React.Children.toArray(children).filter(
            (child) => {
                if (child.props.children === undefined) {
                    return true;
                }
                if (child.props.children.toLowerCase() === "reset")
                {
                    return true;
                }

                return !value || child.props.children.toLowerCase().startsWith(compareValue);
            }
        )}
        </ul>
    </div>
    );
},);


class Search extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userRight: null,
            userLogin: "",
            showMapOptions: false,
            activeCity: "",
            activeCountryIdx: -1,
            activeLat: 54.5920,
            activeLon: 22.2013,
            requireFlyTo: false,
            useMapOptions: false,
            area: [],
            drawArea: false,
            countriesInfo: [],
            elementsInfo: [],
            countriesFilterValue: "",
            elementsPriority: [],
            activeObjects: []
        }
    }

    render() {
        (async () => {
            if (!this.state.userLogin) {
                const responce = await fetch("/api/get_session_info", {
                    method: "POST",
                    headers: { "Accept": "application/json", "Content-Type": "application/json" }
                });

                if (responce.ok) {
                    const sessionInfo = await responce.json();
                    console.log("user status " + sessionInfo.user_rights);

                    if (this.state.userRight != sessionInfo.user_rights) {
                        console.log(sessionInfo.user_login);
                        this.setState({userRight: sessionInfo.user_rights, userLogin: sessionInfo.user_login, requireFlyTo: false});
                    }
                }
                else {
                    console.log("user status developer (debug only)");
                    this.setState({userRight: EUserRight.kAdmin, userLogin: "developer", requireFlyTo: false});
                }
            }

            if (this.state.countriesInfo.length == 0) {
                const cityResponse = await fetch("/api/get_cities", {
                    method: "POST",
                    headers: { "Accept": "application/json", "Content-Type": "application/json" }
                });

                if (cityResponse.ok) {
                    const cityJson = await cityResponse.json();
                    //console.log(cityJson);

                    this.setState({ countriesInfo: cityJson.countries, requireFlyTo: false });
                }
            }

            if (this.state.elementsPriority.length == 0) {
                const elementsResponse = await fetch("/api/get_elements", {
                    method: "POST",
                    headers: { "Accept": "application/json", "Content-Type": "application/json" }
                });

                if (elementsResponse.ok) {
                    const elementsJson = await elementsResponse.json();
                    //console.log(elementsJson);

                    if (this.state.elementsPriority.length != elementsJson.elements.length) {
                        this.setState({
                            elementsInfo: elementsJson.elements,
                            elementsPriority: Array.apply(null, Array(elementsJson.elements.length)).map(function () { return 0; }),
                            requireFlyTo: false
                        });
                    }
                }
            }
        })();

        //console.log("area: " + this.state.area[0]);

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

                        {this.state.activeObjects.length === 0 ? null : this.state.activeObjects.map(obj => {
                            { return (
                            <Marker
                                position={[obj.lat, obj.lon]}
                                icon={MapFeature.DefaultMarkerIcon()}>
                                    <Popup>
                                        {obj.name}
                                    </Popup>
                            </Marker>) }
                        })}
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
        //console.log(this.state.countriesInfo);

        return (
            <div>
                <h1 style={styles.AreaParamsHelperTextSyle}>Search Area</h1>
                {this.renderDropdown(
                    this.state.activeCountryIdx === -1 ? "Country" : this.state.countriesInfo[this.state.activeCountryIdx].name,
                    "2vh",
                    this.state.countriesInfo.map(
                        (country, idx) => (
                            <Dropdown.Item onClick={() => {
                                this.setState({activeCountryIdx: idx, requireFlyTo: false})
                            }}>
                                {country.name}
                            </Dropdown.Item>
                        )
                    ).concat(
                        [<Dropdown.Divider />,
                        <Dropdown.Item onClick={() => {
                            this.setState({activeCountryIdx: -1, requireFlyTo: false})
                        }}>
                            Reset
                        </Dropdown.Item>]
                    )
                )}
                {this.renderDropdown(
                    this.state.activeCity === "" ? "City" : this.state.activeCity,
                    "6vh",
                    this.state.activeCountryIdx === -1 ? [] :
                    this.state.countriesInfo[this.state.activeCountryIdx].cities.map(
                        (city) => {
                            return <Dropdown.Item onClick={() => {
                                this.setState({
                                    activeCity: city.name,
                                    activeLat: city.center.x,
                                    activeLon: city.center.y,
                                    requireFlyTo: true
                                });
                            }}>
                                {city.name}
                            </Dropdown.Item>
                        }
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
                    onClick={async () => {
                        if (this.state.activeCountryIdx === -1) {
                            return;
                        }

                        if (this.state.activeCity === "" && this.state.area.length === 0) {
                            return;
                        }

                        const response = await fetch("/api/perform_request", {
                            method: "POST",
                            headers: { "Accept": "application/json", "Content-Type": "application/json" },
                            body: JSON.stringify({
                                "Account": this.state.userLogin,
                                "Parameters": this.state.elementsInfo.map((elem, idx) => {
                                    return { "Element": elem, "Value": this.state.elementsPriority[idx] + 1 }
                                }),

                                "Border": this.state.area.map((elem, idx) => {
                                    return { "X": elem[0], "Y": elem[1] }
                                }),

                                "Country": this.state.countriesInfo[this.state.activeCountryIdx].name,
                                "City": this.state.activeCity
                            })
                        });

                        if (response.ok) {
                            const answer = await response.json();
                            let newActiveObjects = answer.objects.map(e => {
                                return {name: e.name, lat: e.coord.x, lon: e.coord.y};
                            });
                            
                            this.setState({activeObjects: newActiveObjects});
                            console.log(newActiveObjects);
                        }
                    }}>
                    <p style={styles.ButtonTextStyle}>Search</p>
                </button>
                <button
                    type="button"
                    style={styles.SearchManageSaveButton}
                    onClick={async () => {
                        if (this.state.activeCountryIdx === -1) {
                            return;
                        }

                        if (this.state.activeCity === "" && this.state.area.length === 0) {
                            return;
                        }

                        const response = await fetch("/api/save_request", {
                            method: "POST",
                            headers: { "Accept": "application/json", "Content-Type": "application/json" },
                            body: JSON.stringify({
                                "Account": this.state.userLogin,
                                "Parameters": this.state.elementsInfo.map((elem, idx) => {
                                    return { "Element": elem, "Value": this.state.elementsPriority[idx] + 1 }
                                }),

                                "Border": this.state.area.map((elem, idx) => {
                                    return { "X": elem[0], "Y": elem[1] }
                                }),

                                "Country": this.state.countriesInfo[this.state.activeCountryIdx].name,
                                "City": this.state.activeCity
                            })
                        });

                        if (response.ok) {
                            const answer = await response.json();
                        }
                    }}>
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
            <div style={{position: "absolute", left: "2vw", top: top, width: "20vw", height: "50vh"}}>
                <Dropdown>
                    <Dropdown.Toggle
                        variant="success"
                        id="cityDropdown">
                        {toggleString}
                    </Dropdown.Toggle>

                    <Dropdown.Menu as={FilterMenu}>
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
            {this.state.elementsInfo.map((element, idx) => (
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
