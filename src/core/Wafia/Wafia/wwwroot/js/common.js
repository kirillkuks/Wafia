import React from "react";
import * as styles from "./styles.js"

export const EScreenState = {
    kIdle: 0,
    kLogIn: 1,
    kSignUp: 2
};

export const EUserRight = {
    kGuest: "guest",
    kUser: "user",
    kAdmin: "admin"
};

export const EHtmlPages = {
    kGuestScreen: "../GuestScreen.html",
    kPersonalArea: "../PersonalArea1.html",
    kAbout: "../About.html"
};

export const ClientConsts = {
    kMaxLoginLength: 50,
    kMaxPasswordLength: 20
};


export function PersonalAreaRedirectButton() {
    return (
        <button
            onClick={() => {
                window.location.assign(EHtmlPages.kPersonalArea);
            }}
            type="button"
            style={styles.PersonalAreaButtonIconStyle}>
            <img src={"../img/personalArea.png"}></img>
        </button>
    )
}
