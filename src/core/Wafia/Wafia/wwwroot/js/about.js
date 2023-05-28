'use strict';
import React from "react";
import ReactDOM from "react-dom";
import "../css/reset.css";

import * as styles from "./styles.js";
import { EScreenState, EUserRight, EHtmlPages } from "./common.js";
import LogInWindowPopUpCreaterComponent from "./logInPopUp.js";
import { PersonalAreaRedirectButton } from "./common.js";


class About extends LogInWindowPopUpCreaterComponent {
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
                    <h1>AboutPrikol</h1>
                </div>

                <a
                    href={EHtmlPages.kGuestScreen}
                    style={styles.BackToGuestScreenButtonStyle}>
                    <img src="../img/backToGuestScreen.png"></img>
                </a>
                {this.renderLogInButton()}
            </header>
        );
    }

    renderMain() {
        return (
            <main style={styles.BodyStyle}>
                
                <div style={styles.AboutTextStyle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                    <p>Spaghetti Carbonara, one of the most famous Pasta Recipes of Roman Cuisine, is made only with 5 simple ingredients: spaghetti seasoned with browned guanciale, black pepper, pecorino Romano and beaten eggs.
                        In the authentic Italian recipe for carbonara, the ingredients are very few and of excellent quality. The high quality of ingredients is a necessary condition for the success of this recipe.
                        In spite of many beliefs, the ingredients of the traditional recipe are only 5: guanciale, pecorino Romano, eggs, pepper and spaghetti. To make the best carbonara of your life, you don't need any other ingredients, so
                        DO NOT USE garlic, parsley, onion, cream, milk, parmigiano, pancetta, bacon. OK?
                    </p> 
                </div>
                
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

            if (this.state.userRight != sessionInfo.user_rights) {
                this.setState({ userRight: sessionInfo.user_rights });
            }
        })();

        return (
            <div>
                {this.renderHeader()}
                {this.renderMain()}
            </div>
        );
    }
}

const domContainer = document.getElementById("Main");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(About));
