import React from "react";
import * as styles from "./styles.js";
import { EScreenState, EHtmlPages, ClientConsts } from "./common.js";


export default class LogInWindowPopUpCreaterComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            screenState: EScreenState.kIdle,

            isLogInError: false,
            logInErrorMessage: "",
            isSignUpError: false,
            signUpErrorMessage: ""
        };
    }

    render() {
        if (this.state.screenState === EScreenState.kLogIn) {
            return this.renderLogInWindow();
        }
        else if (this.state.screenState === EScreenState.kSignUp) {
            return this.renderSignUpWindow();
        }

        return null;
    }

    renderLogInWindow() {
        return (
            <section style={styles.PopUpWindowStyle}>
                <h2 style={styles.PopUpHeaderTextStyle}>Log In</h2>
                <input
                    maxLength={ClientConsts.kMaxLoginLength}
                    style={styles.LogInFieldLoginStyle}
                    id="LoginField"
                    placeholder="Name">
                </input>
                <input
                    type="password"
                    maxLength={ClientConsts.kMaxPasswordLength}
                    style={styles.PasswordFieldLoginStyle}
                    id="PasswordField"
                    placeholder="Password">
                </input>
                <button
                    style={styles.ExitButtonStyle}
                    onClick={() => {
                        this.setState({ screenState: EScreenState.kIdle, isLogInError: false, isSignUpError: false });
                    }}>
                    <img src="../img/exit.png"></img>
                </button>
                {this.state.isLogInError ?
                <h2 style={styles.WarningTextStyle}>{this.state.logInErrorMessage}</h2> :
                null}
                <button
                    style={styles.LogInNextButtonStyle}
                    onClick={async () => {
                        const response = await fetch("/api/login", {
                            method: "POST",
                            headers: { "Accept": "application/json", "Content-Type": "application/json" },
                            body: JSON.stringify({
                                "Login": document.getElementById("LoginField").value,
                                "Mail": "prikol",
                                "Password": document.getElementById("PasswordField").value
                            })
                        });
                
                        if (response.ok) {
                            const answer = await response.json();
                            window.location.assign(EHtmlPages.kPersonalArea);
                        }
                        else {
                            this.setState({isLogInError: true, logInErrorMessage: "Invalid login or password"});
                        }
                    }}>
                    <img src="../img/arrowNext.png"></img>
                </button>
                <button style={styles.DontHaveAccountButtonStyle}
                    onClick={() => {
                        this.setState({screenState: EScreenState.kSignUp});
                    }}>
                    <p style={styles.ButtonTextStyle}>Don't have an account?</p>
                </button>
            </section>
        );
    }

    renderSignUpWindow() {
        return (
            <section style={styles.PopUpWindowStyle}>
                <h2 style={styles.PopUpHeaderTextStyle}>Sign Up</h2>
                {this.state.isSignUpError ?
                <h2 style={styles.WarningTextStyle}>{this.state.signUpErrorMessage}</h2> :
                null}
                <input
                maxLength={ClientConsts.kMaxLoginLength}
                style={styles.SignUpFieldLoginStyle}
                id="SignUpLoginField"
                placeholder="Login">
                </input>
                <input
                maxLength={ClientConsts.kMaxLoginLength}
                style={styles.SignUpFieldMailStyle}
                id="SignUpMailField"
                placeholder="Email">
                </input>
                <input
                type="password"
                maxLength={ClientConsts.kMaxPasswordLength}
                style={styles.SignUpFieldPasswordStyle}
                id="SignUpPasswordField"
                placeholder="Password">
                </input>
                <input
                type="password"
                maxLength={ClientConsts.kMaxPasswordLength}
                style={styles.SignUpFieldRepeatPasswordStyle}
                id="SignUpRepeatPasswordField"
                placeholder="Repeat password">
                </input>
                <button
                    style={styles.ExitButtonStyle}
                    onClick={() => {
                        this.setState({ screenState: EScreenState.kIdle, isLogInError: false, isSignUpError: false });
                    }}>
                    <img src="../img/exit.png"></img>
                </button>
                <button
                    style={styles.LogInNextButtonStyle}
                    onClick={async () => {
                        let password = document.getElementById("SignUpPasswordField").value;
                        let checkPassword = document.getElementById("SignUpRepeatPasswordField").value;

                        if (password === checkPassword) {
                            const response = await fetch("/api/signup", {
                                method: "POST",
                                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                                body: JSON.stringify({
                                    "Login": document.getElementById("SignUpLoginField").value,
                                    "Mail": document.getElementById("SignUpMailField").value,
                                    "Password": password
                                })
                            });
                    
                            if (response.ok) {
                                console.log("success sign up");
                                const answer = await response.json();
                                window.location.assign(EHtmlPages.kPersonalArea);
                            }
                            else {
                                this.setState({isSignUpError: true, signUpErrorMessage: "User with such is login already exist"});
                            }
                        }
                        else {
                            this.setState({isSignUpError: true, signUpErrorMessage: "Passsword mismatch"})
                        }

                    }}>
                    <img src="../img/arrowNext.png"></img>
                </button>
            </section>
        );
    }
}
