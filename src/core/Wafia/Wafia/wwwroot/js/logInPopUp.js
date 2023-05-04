'use strict';

const e = React.createElement;


class LogInPopUp extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <button>
                <p>LogIn</p>
            </button>
        );
    }
}

const logInPopUp = document.getElementById("LogInPopUp");
const logInPopUpRoot = ReactDOM.createRoot(logInPopUp);
root.render(React.createElement(LogInPopUp));
