import React from "react";
import { Marker, Popup, useMap, useMapEvents } from "react-leaflet";

import markerIconPng from "../img/marker-icon.png";
import { Icon } from "leaflet";


export function DefaultMarkerIcon() {
    return new Icon({iconUrl: markerIconPng, iconSize: [25, 41], iconAnchor: [12, 41]});
}


export function FindLocation() {
    const [position, setPosition] = React.useState(null);
    const map = useMapEvents({
        click() {
            map.locate();
        },
        locationfound(e) {
            setPosition(e.latlng);
            map.flyTo(e.latlng, map.getZoom());
        }
    });

    return position === null ? null : (
        <Marker
            position={position}
            icon={DefaultMarkerIcon()}>
            <Popup>You are here</Popup>
        </Marker>
    );
}

export function MoveTo({ lat, lon }) {

    const map = useMap();
    console.log("prikol" + lat + " | " + lon);
    map.flyTo([lat, lon], map.getZoom());

    return null;
}
