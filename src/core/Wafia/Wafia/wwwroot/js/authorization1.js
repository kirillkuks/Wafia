'use strict';

const e = React.createElement;

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


class Authorization extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isLogIn: false };
    }

    outputString() {
        if (this.state.isLogIn) {
            return <p style={ButtonTextStyle}>Sign In1</p>
        } else {
            return <p style={ButtonTextStyle}>Sign In2</p>
        }
    }

    renderLogIn() {
        if (this.state.isLogIn) {
            return <button
                onClick={() => window.location.assign("../PersonalArea.html")}
                type="button"
                style={LogInButtonStyle}>
                    {this.render}
                    <p style={ButtonTextStyle}>Personal42 Area</p>
            </button>
        }

        return <button
            onClick={() => this.setState({ isLogIn: true })}
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

                {this.renderLogIn()}
            </header>
        );
    }

    render() {
        return this.renderHeader();
    }
}

const domContainer = document.getElementById("Header");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Authorization));
