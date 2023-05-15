const path = require("path");

let config = {
    mode: 'development',
    module: {
        rules: [
        {
            test: /\.(js|jsx)$/,
            exclude: /node_modules/,
            use: {
            loader: "babel-loader",
            },
        },
        {
            test: /\.(sa|sc|c)ss$/,
            use: ["style-loader", "css-loader", "sass-loader"],
        },
        {
            test: /\.(png|woff|woff2|eot|ttf|svg)$/,
            loader: "url-loader",
            options: { limit: false },
        },
        ],
    },
}


let guestScreen = Object.assign({}, config, {
    entry: './wwwroot/js/authorization1.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "authorization1.js",
    }
});

let personalArea = Object.assign({}, config, {
    entry: './wwwroot/js/personalArea.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "personalArea.js"
    }
});

let about = Object.assign({}, config, {
    entry: './wwwroot/js/about.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "about.js"
    }
});

module.exports = [
    guestScreen,
    personalArea,
    about
];