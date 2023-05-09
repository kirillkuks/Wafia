'use strict';
import React from "react";
import ReactDOM from "react-dom";
import * as styles from "./styles.js";
import "../css/reset.css";


class Authorization extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isLogIn: false,
            isLoggingIn: false,
            invalidLoginMsg: false
        };
    }

    outputString() {
        if (this.state.isLogIn) {
            return <p style={styles.ButtonTextStyle}>Sign In2</p>
        } else {
            return <p style={styles.ButtonTextStyle}>Sign In</p>
        }
    }



    renderLogInButton() {
        let state = "uninit"; 
        console.log(state);

        (async () => {
            const responce = await fetch("/api/get_user_rights", {
                method: "POST",
                headers: { "Accept": "application/json", "Content-Type": "application/json" }
            })

            state = "init";
            console.log(state);
        })();

        return <button
                onClick={() => {
                        this.setState({ isLoggingIn: true });
                    }
                }
                type="button"
                style={styles.LogInButtonStyle}>
                    { state === "uninit" ? this.outputString() : <p style={styles.ButtonTextStyle}>FAIL</p>}
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
        if (this.state.isLoggingIn) {
            return (
                <section style={styles.LogInWindowStyle}>
                    <h2 style={styles.LogInTextStyle}>Sing In</h2>
                    <input
                        maxlength="100"
                        style={styles.LogInFieldLoginStyle}
                        id="LoginField"
                        placeholder="Name">
                    </input>
                    <input
                        type="password"
                        maxlength="100"
                        style={styles.PasswordFieldLoginStyle}
                        id="PasswordField"
                        placeholder="Password">
                    </input>
                    <button
                        style={styles.ExitButtonStyle}
                        onClick={() => {
                            this.setState({ isLoggingIn: false, isLoginInvalid: false })
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
