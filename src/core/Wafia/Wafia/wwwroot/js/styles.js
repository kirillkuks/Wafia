function BuildStyle(parentStyle, childStyle) {
    return Object.assign({}, parentStyle, childStyle);
}


export const BodyStyle = {
    backgroundColor: "#F2F1E1"
}

export const LogInButtonStyle = {
    position: "absolute",
    width: "190px",
    height: "85px",
    right: "0px",
    top: "0px",

    border: "0",
    backgroundColor: "#E94C8D"
}

export const AboutButtonStyle = {
    "position": "absolute",
    "width": "190px",
    "height": "85px",
    "left": "0px",
    "top": "0px",

    "border": "0",
    "backgroundColor": "#E94C8D"
}

export const ExitButtonStyle = {
    "position": "absolute",
    "right": "0%",
    "top": "0%",
    "backgroundColor": "#FFFFFF",
    "transform": "scale(0.5)",
    "border": "none" 
}

export const LogInNextButtonStyle = {
    "position": "absolute",
    "left": "50%",
    "top": "65%",
    "backgroundColor": "#FFFFFF",
    "transform": "translate(-50%, 0%) scale(0.75)",
    "border": "none" 
}

export const DontHaveAccountButtonStyle = {
    "position": "absolute",
    "width": "30vw",
    "height": "5vh",
    "left": "50%",
    "top": "80%",
    "transform": "translate(-50%, 0%)",

    "border": "0",
    "background": "#E94C8D",

    "boxShadow": "0px 8px 8px rgba(0, 0, 0, 0.5)",
    "filter": "blur(0.5px)",
    "borderRadius": "15px"
}

export const ButtonTextStyle = {
    "fontFamily": 'Inter',
    "fontStyle": "normal",
    "fontWeight": "400",
    "fontSize": "28px",
    "lineHeight": "34px",
    "textAlign": "center",

    "color": "#000000",
    "textShadow": "2px 2px 4px rgba(0, 0, 0, 0.5)"
}

export const WarningTextStyle = {
    "position": "absolute",
    "top": "13%",
    "left": "19%",
    "transform": "translate(0%, +50%)",
    "fontFamily": 'Inter',
    "fontStyle": "normal",
    "fontWeight": "300",
    "fontSize": "20px",
    "lineHeight": "34px",

    "color": "#FF0000"
}


const MenuButtonStyle = {
    position: "absolute",
    border: "0",
    background: "#E94C8D",

    boxShadow: "0px 8px 8px rgba(0, 0, 0, 0.5)",
    filter: "blur(0.5px)",
    borderRadius: "15px"
}

export const SearchButtonStyle = BuildStyle(MenuButtonStyle, {
    width: "15vw",
    height: "10vh",
    left: "10vw",
    top: "76vh",
});

const PersonalAreaButtonStyle = BuildStyle(MenuButtonStyle, {
    width: "17vw",
    height: "8vh"
});

export const UpdateDatabaseButton = BuildStyle(PersonalAreaButtonStyle, {
    left: "50%",
    top: "20%",
    transform: "translate(-50%, -50%)"
});

export const GiveRightsButton = BuildStyle(PersonalAreaButtonStyle, {
    left: "50%",
    top: "85%",
    transform: "translate(-50%, -50%)"
});


export const HeaderStyle = {
    position: "relative",
    top: "0px",
    height: "85px",
    "backgroundImage": "url(../img/baseHeader.png)",
    "backgroundSize": "contain"
}

export const HeaderTitle = {
    "position": "absolute",
    "left": "50%",
    "top": "25%",
    "transform": "translate(-50%, 25%)",
    "fontSize": "24px",
    "color": "white"
}

export const InterctiveMapStyle = {
    "position": "absolute",
    "width": "59vw",
    "height": "63vh",
    "left": "36vw",
    "top": "22vh",
    "backgroundColor": "#D9D9D9"
}

export const PopUpWindowStyle = {
    "position": "absolute",
    "z-index": "100000",
    "width": "40vw",
    "height": "60vh",
    "borderRadius": "20px",
    "backgroundColor": "#FFFFFF",
    "top": "50%",
    "left": "50%",
    "transform": "translate(-50%, -50%)",
    "boxShadow": "0px 8px 8px rgba(0, 0, 0, 0.5)"
}

export const PopUpHeaderTextStyle = {
    "position" : "relative",
    "fontFamily": 'Inter',
    "fontStyle": "normal",
    "fontWeight": "500",
    "fontSize": "28px",
    "lineHeight": "34px",
    "textAlign": "center",
    "top": "10%",

    "color": "#000000"
}

export const UserLoginTextStyle = {
    position: "absolute",
    fontSize: "24px",
    fontFamily: "Inter",
    textAlign: "right",
    transform: "translate(-100%, -25%)",

    color: "#FFFFFF"
}

export const UserLoginDropdownStyle = {
    position: "absolute",
    top: "50%",
    right: "1vw",
    transform: "translate(0%, -50%)",
}

export const AboutTextStyle = {
    "position": "absolute",
    "fontFamily": 'Inter',
    "fontStyle": "normal",
    "fontWeight": "500",
    "fontSize": "24px",
    "lineHeight": "46px",
    "textAlign": "center",

    "top": "15%",
    "left": "25%",
    "width": "50%",
    "height": "70%",

    "color": "#000000"
}

export const PesonalAreaBordersStyle = {
    position: "relative",
    left: "50%",
    transform: "translate(-50%, 0%)",
    width: "33vw",
    height: "91vh",
}

const InputButton = {
    position : "relative",
    left: "50%",
    transform: "translate(-50%, 0%)",
    borderRadius: "7px",
    width: "25vw",
    height: "5vh"
}

export const LogInFieldLoginStyle = BuildStyle(InputButton, {
    top: "20%"
});

export const PasswordFieldLoginStyle = BuildStyle(InputButton, {
    top: "25%"
});

export const SignUpFieldLoginStyle = BuildStyle(InputButton, {
    top: "17%"
});

export const SignUpFieldMailStyle = BuildStyle(InputButton, {
    top: "20%"
});

export const SignUpFieldPasswordStyle = BuildStyle(InputButton, {
    top: "23%"
});

export const SignUpFieldRepeatPasswordStyle = BuildStyle(InputButton, {
    top: "26%"
});


export const HistoryFieldStyle = {
    position: "absolute",
    left: "5vw",
    top: "10%",
    transform: "scale(0.75)"
}

export const HistoryBackgroundStyle = {
    position: "absolute",
    left: "5vw",
    top: "15%",
    width: "25%",
    height: "75%",
    backgroundColor: "FFFFFF",
    border: "2px solid",
    borderRadius: "10px",
}

export const DeleteFromHistoryButtonStyle = {
    position: "relative",
    left: "-15%",
    backgroundColor: "#FFFFFF",
    border: "none",
    transform: "scale(0.90)"
}

export const PersonalAreaButtonIconStyle = {
    position: "absolute",
    width: "85px",
    height: "85px",
    right: "0px",
    top: "0px",
    border: "none",
    background: "none"
}

export const BackToGuestScreenButtonStyle = {
    position: "absolute",
    width: "85px",
    height: "85px",
    left: "0px",
    top: "0px",
    boredr: "none",
    background: "none"
}

export const SearchParamsBackgroundStyle = {
    position: "absolute",
    left: "0px",
    top: "85px",
    width: "25vw",
    height: "calc(100vh - 85px)",
    backgroundColor: "#D9F2E2",
    borderRight: "2px solid",
}


const HelperTextStyle = {
    position: "absolute",
    fontFamily: "Inter",
    fontStyle: "Italic",
    fontSize: "12px",
    fontWeight: "200",
    lineHeight: "15px",
    textAlign: "left",
    color: "#000000",
    width: "24vw",
    height: "2vh",
    left: "1vw"
}

export const AreaParamsHelperTextSyle = BuildStyle(HelperTextStyle, {
    top: "0vh"
});

export const InfrastructureElementsHelperTextStyle = BuildStyle(HelperTextStyle, {
    top: "29vh"
});


const SearchManageButton = {
    position: "absolute",
    border: "0",
    background: "#E94C8D",
    boxShadow: "0px 8px 8px rgba(0, 0, 0, 0.5)",
    filter: "blur(0.5px)",
    borderRadius: "15px"
}

export const SearchManageSearchButton = BuildStyle(SearchManageButton, {
    left: "5%",
    right: "65.83%",
    top: "88.44%",
    bottom: "3.95%"
});

export const SearchManageSaveButton = BuildStyle(SearchManageButton, {
    left: "65.83%",
    right: "5%",
    top: "88.44%",
    bottom: "3.95%"
});

export const SearchManageMapOptionsButton = BuildStyle(SearchManageButton, {
    left: "29.17%",
    right: "29.17%",
    top: "79.7%",
    bottom: "15.28%"
});


export const InfrastructureElementsListStyle = {
    position: "absolute",
    left: "0vw",
    top: "31vh",
    width: "calc(25vw - 1px)",
    height: "41vh",
    overflowY: "scroll",
    overflowX: "clip"
}

export const InfrastructureElementPriorityStyle = {
    position: "absolute",
    width: "21vw",
    height: "2vh",
    left: "1vw",
}

export const InfrastructureElementNameStyle = {
    position: "absolute",
    width: "21vw",
    height: "4vh",
    left: "1vw",

    fontFamily: "Inter",
    fontStyle: "normal",
    fontWeight: "400",
    fontSize: "28px",
    lineHeight: "34px",
    display: "flex",
    alignItems: "center",

    color: "#000000",
}


export const SpecificObjectParamsStyle = {
    position: "absolute",
    top: "15vh",
    left: "0vw",
    width: "25vw",
    height: "13vh",

    backgroundColor: "#FF0000"
}


export const MapOptionsStyle = {
    position: "absolute",
    width: "min(24vh, 13vw)",
    height: "min(24vh, 13vw)",
    left: "80vw",
    top: "24vh",

    zIndex: "100000",
    backgroundColor: "#FFFFFF",
    border: "2px solid"
}

export const MapOptionsPointsStyle = {
    position: "absolute",
    left: "10.94%",
    right: "10.94%",
    top: "36.33%",
    bottom: "50%"
}

const MapOptionsButtonStyle = {
    position: "absolute",
    left: "16.41%",
    right: "16.41%",

    filter: "blur(0.5px)",
    borderRadius: "15px",
    boxShadow: "0px 8px 8px rgba(0, 0, 0, 0.5)"
}

export const MapOptionsSubmitAreaStyle = BuildStyle(MapOptionsButtonStyle, {
    top: "70.7%",
    bottom: "11.72%",
});

export const MapOptionsResetAreaStyle = BuildStyle(MapOptionsButtonStyle, {
    top: "9.38%",
    bottom: "73.05%"
});


export const DropdownFilterStyle = {
    position: "absolute",
    height: "25vh",
    overflowY: "scroll"
}
