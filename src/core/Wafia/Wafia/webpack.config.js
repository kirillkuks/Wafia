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
        {
            test: /\.m?js$/,
            enforce: 'pre',
            use: ['source-map-loader'],
        },
        ],
    },
}


const guestScreen = Object.assign({}, config, {
    entry: './wwwroot/js/authorization1.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "authorization.js"
    }
});

const personalArea = Object.assign({}, config, {
    entry: './wwwroot/js/personalArea.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "personalArea.js"
    }
});

const search = Object.assign({}, config, {
    entry: './wwwroot/js/search.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"),
        filename: "search.js"
    }
});


module.exports = [
    guestScreen,
    personalArea,
    search
];