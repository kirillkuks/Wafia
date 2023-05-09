const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = {
    entry: './wwwroot/js/authorization1.js',
    output: {
        path: path.join(__dirname, "/wwwroot/dst"), // the bundle output path
        filename: "authorization1.js", // the name of the bundle
    },
    mode: 'development',
    module: {
        rules: [
        {
            test: /\.(js|jsx)$/, // .js and .jsx files
            exclude: /node_modules/, // excluding the node_modules folder
            use: {
            loader: "babel-loader",
            },
        },
        {
            test: /\.(sa|sc|c)ss$/, // styles files
            use: ["style-loader", "css-loader", "sass-loader"],
        },
        {
            test: /\.(png|woff|woff2|eot|ttf|svg)$/, // to import images and fonts
            loader: "url-loader",
            options: { limit: false },
        },
        ],
    },
};