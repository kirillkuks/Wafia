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


class Authorization extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isLogIn: false };
    }

    render() {
        if (this.state.isLogIn) {
            return (
                <button
                onClick={() => window.location.assign("../PersonalArea.html")}
                type="button"
                style={LogInButtonStyle}>
                    <p style={ButtonTextStyle}>Personal Area</p>
                </button>
              );
        }

        return (
            <button
            onClick={() => this.setState({ isLogIn: true })}
            type="button"
            style={LogInButtonStyle}>
                <p style={ButtonTextStyle}>Sign In</p>
            </button>
          );
    }
}

const domContainer = document.getElementById("log_in_button");
const root = ReactDOM.createRoot(domContainer);
root.render(React.createElement(Authorization))