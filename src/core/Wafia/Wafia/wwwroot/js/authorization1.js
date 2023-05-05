'use strict';

const LogInButtonStyle = {
    position: "absolute",
    width: "190px",
    height: "85px",
    right: "0px",
    top: "0px",

    border: "0",
    "background-color": "#E94C8D"
}

const AboutButtonStyle = {
    "position": "absolute",
    "width": "190px",
    "height": "85px",
    "left": "0px",
    "top": "0px",

    "border": "0",
    "background-color": "#E94C8D"
}

const ExitButtonStyle = {
    "position": "absolute",
    "right": "0%",
    "top": "0%",
    "background-color": "#FFFFFF",
    "transform": "scale(0.5)",
    "border": "none" 
}

const LogInNextButtonStyle = {
    "position": "absolute",
    "left": "50%",
    "top": "65%",
    "background-color": "#FFFFFF",
    "transform": "translate(-50%, 0%) scale(0.75)",
    "border": "none" 
}

const DontHaveAccountButtonStyle = {
    "position": "absolute",
    "width": "30vw",
    "height": "5vh",
    "left": "50%",
    "top": "80%",
    "transform": "translate(-50%, 0%)",

    "border": "0",
    "background": "#E94C8D",

    "box-shadow": "0px 8px 8px rgba(0, 0, 0, 0.5)",
    "filter": "blur(0.5px)",
    "border-radius": "15px"
}

const ButtonTextStyle = {
    "font-family": 'Inter',
    "font-style": "normal",
    "font-weight": "400",
    "font-size": "28px",
    "line-height": "34px",
    "text-align": "center",

    "color": "#000000",
    "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)"
}

const WarningTextStyle = {
    "position": "absolute",
    "top": "15%",
    "left": "19%",
    "transform": "translate(0%, +50%)",
    "font-family": 'Inter',
    "font-style": "normal",
    "font-weight": "300",
    "font-size": "20px",
    "line-height": "34px",

    "color": "#FF0000",
}

const SearchButtonStyle = {
    "position": "absolute",
    "width": "15vw",
    "height": "10vh",
    "left": "10vw",
    "top": "76vh",

    "border": "0",
    "background": "#E94C8D",

    "box-shadow": "0px 8px 8px rgba(0, 0, 0, 0.5)",
    "filter": "blur(0.5px)",
    "border-radius": "15px"
}

const HeaderStyle = {
    position: "relative",
    top: "0px",
    height: "85px",
    "background-image": "url(../img/baseHeader.png)",
    "background-size": "contain"
}

const HeaderTitle = {
    "position": "absolute",
    "left": "50%",
    "top": "25%",
    "transform": "translate(-50%, 25%)",
    "font-size": "24px",
    "color": "white"
}

const InterctiveMapStyle = {
    "position": "absolute",
    "width": "59vw",
    "height": "63vh",
    "left": "36vw",
    "top": "22vh",
    "background-color": "#D9D9D9"
}

let LogInWindowStyle = {
    "position": "absolute",
    "width": "40vw",
    "height": "60vh",
    "border-radius": "20px",
    "background-color": "#FFFFFF",
    "top": "50%",
    "left": "50%",
    "transform": "translate(-50%, -50%)",
    "box-shadow": "0px 8px 8px rgba(0, 0, 0, 0.5)",
}

const LogInTextStyle = {
    "position" : "relative",
    "font-family": 'Inter',
    "font-style": "normal",
    "font-weight": "500",
    "font-size": "28px",
    "line-height": "34px",
    "text-align": "center",
    "top": "10%",

    "color": "#000000",
}

const LogInFieldLoginStyle = {
    "position" : "relative",
    "top": "20%",
    "left": "50%",
    "transform": "translate(-50%, 0%)",
    "border-radius": "7px",
    "width": "25vw",
    "height": "5vh"
}

const PasswordFieldLoginStyle = {
    "position" : "relative",
    "top": "25%",
    "left": "50%",
    "transform": "translate(-50%, 0%)",
    "border-radius": "7px",
    "width": "25vw",
    "height": "5vh"
}


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
            return <p style={ButtonTextStyle}>Sign In2</p>
        } else {
            return <p style={ButtonTextStyle}>Sign In</p>
        }
    }

    renderLogInButton() {
        return <button
                onClick={() => {
                        this.setState({ isLoggingIn: true });
                    }
                }
                type="button"
                style={LogInButtonStyle}>
                    {this.outputString()}
                </button>
    }


    renderHeader() {
        return (
            <header style={HeaderStyle}>
                <div style={HeaderTitle}>
                    <h1>Web Application For Infrastructure Analyze</h1>
                </div>

                <a href="../About.html">
                    <button style={AboutButtonStyle}>
                        <p style={ButtonTextStyle}>About</p>
                    </button>
                </a>

                {this.renderLogInButton()}
            </header>
        );
    }

    renderMain() {
        return (
            <main>
                <button
                    style={SearchButtonStyle}
                    onClick={async () => {
                        const response = await fetch("/api/search", {
                            method: "GET",
                            headers: { "Accept": "application/json" }
                        });
                
                        if (response.ok) {
                            window.location.assign("https://www.youtube.com/watch?v=ywthKNqI7uI");
                        }
                    }}>
                    <p style={ButtonTextStyle}>Search</p>
                </button>
                <section style={InterctiveMapStyle}></section>
                {this.renderLogInWindow()}
            </main>
        );
    }

    renderLogInWindow() {
        if (this.state.isLoggingIn) {
            return (
                <section style={LogInWindowStyle}>
                    <h2 style={LogInTextStyle}>Sing In</h2>
                    <input
                        maxlength="100"
                        style={LogInFieldLoginStyle}
                        id="LoginField"
                        placeholder="Name">
                    </input>
                    <input
                        type="password"
                        maxlength="100"
                        style={PasswordFieldLoginStyle}
                        id="PasswordField"
                        placeholder="Password">
                    </input>
                    <button
                        style={ExitButtonStyle}
                        onClick={() => {
                            this.setState({ isLoggingIn: false, isLoginInvalid: false })
                        }}>
                        <img src="../img/exit.png"></img>
                    </button>
                    {this.state.isLoginInvalid ?
                    <h2 style={WarningTextStyle}>Invalid login or password</h2> :
                    null}
                    <button
                        style={LogInNextButtonStyle}
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
                    <button style={DontHaveAccountButtonStyle}>
                        <p style={ButtonTextStyle}>Don't have an account?</p>
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

const domContainer = document.getElementById("Header");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Authorization));
