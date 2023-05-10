'use strict';
import React from "react";
import ReactDOM from "react-dom";
import * as styles from "./styles.js";
import "../css/reset.css";


const EGuestScreenState = { kIdle: 0, kLogIn: 1, kSignUp: 2 };
const EUserRight = { kGuest: "guest", kUser: "user", kAdmin: "admin" };


class Authorization extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            invalidLoginMsg: false,
            screenState: EGuestScreenState.kIdle,
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
                this.setState({ screenState: EGuestScreenState.kLogIn })
            }
        }

        return () => {
            window.location.assign("../PersonalArea.html");
        }
    }

    renderLogInButton() {
        return <button
                onClick={this.logInButtonOnClick()}
                type="button"
                style={styles.LogInButtonStyle}>
                    { this.logInButtonOutputString() }
                </button>
    }


    renderHeader() {
        return (
            <header style={styles.HeaderStyle}>
                <div style={styles.HeaderTitle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                </div>

                <a href="../About.html">
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
                <section style={styles.InterctiveMapStyle}></section>
                {this.renderLogInWindow()}
            </main>
        );
    }

    renderLogInWindow() {
        if (this.state.screenState == EGuestScreenState.kLogIn) {
            return (
                <section style={styles.LogInWindowStyle}>
                    <h2 style={styles.LogInTextStyle}>Sing In</h2>
                    <input
                        maxLength="100"
                        style={styles.LogInFieldLoginStyle}
                        id="LoginField"
                        placeholder="Name">
                    </input>
                    <input
                        type="password"
                        maxLength="100"
                        style={styles.PasswordFieldLoginStyle}
                        id="PasswordField"
                        placeholder="Password">
                    </input>
                    <button
                        style={styles.ExitButtonStyle}
                        onClick={() => {
                            this.setState({ screenState: EGuestScreenState.kIdle, isLoginInvalid: false })
                        }}>
                        <img src="../img/exit.png"></img>
                    </button>
                    {this.state.isLoginInvalid ?
                    <h2 style={styles.WarningTextStyle}>Invalid login or password</h2> :
                    null}
                    <button
                        style={styles.LogInNextButtonStyle}
                        onClick={async () => {
                            const response = await fetch("/api/login", {
                                method: "POST",
                                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                                body: JSON.stringify({
                                    "Login": document.getElementById("LoginField").value,
                                    "Password": document.getElementById("PasswordField").value
                                })
                            });
                    
                            if (response.ok) {
                                const answer = await response.json();
                                window.location.assign("../PersonalArea.html");
                            }
                            else {
                                this.setState({isLoginInvalid: true});
                            }
                        }}>
                        <img src="../img/arrowNext.png"></img>
                    </button>
                    <button style={styles.DontHaveAccountButtonStyle}>
                        <p style={styles.ButtonTextStyle}>Don't have an account?</p>
                    </button>
                </section>
            );
        }

        return null;
    }

    render() {
        (async () => {
            const responce = await fetch("/api/get_user_rights", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            })

            const answer = await responce.json();
            console.log("user status " + answer.state);

            if (this.state.userRight != answer.state)
            {
                this.setState({userRight: answer.state});
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
