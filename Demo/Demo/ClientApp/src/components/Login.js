import React, { Component } from 'react';
import axios from 'axios';
import { connect } from 'react-redux';


export default class Login extends Component {

    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: ""
        }
        this.handleInputChange = this.handleInputChange.bind(this);
        //this.handleOnSubmit = this.handleOnSubmit.bind(this);
        
    }

    handleInputChange = event => {
        this.setState({
            ...this.state,
            [event.target.id]: event.target.value
        });
    }

     handleOnSubmit = async()=> {
        console.log(this.state)
         const response = await axios.post("https://localhost:44374/api/Employee/Login", this.state.username, this.state.password, {
            'Content-Type': 'application/json'
        })
        const result = await response.data;
        alert(result);
        if (result > 0) {
            //this.setState({
            //    popupState: {
            //        ...this.state.popupState,
            //        message: "User information saved successfully.",
            //        isshow: true,
            //        redirect: true
            //    }
            //})
        }
        else {
            //this.setState({
            //    popupState: {
            //        ...this.state.popupState,
            //        message: "An error has occurred. Please contact to your administrator.",
            //        isshow: true,
            //        redirect: false
            //    }
            //})
        }
    };

    render() {
        return (
            <div className="container-fluid">
                <div className="user-list">
                    <div className="page-header">
                        <h2><strong>Sign In</strong></h2>
                    </div>
                    <div className="main-content">
                        <div className="main-search-block">
                            <div className="row">
                                <div className="col-md-6 col-lg-4">
                                    <div className="form-group">
                                        <label className="col-form-label">User Name :</label>
                                        <input type="text" className="form-control" id="username" ref="username" value={this.state.username} onChange={this.handleInputChange} />
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md-6 col-lg-4">
                                    <div className="form-group">
                                        <label className="col-form-label">Password :</label>
                                        <input type="password" className="form-control" id="password" ref="password" value={this.state.password} onChange={this.handleInputChange} />
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md-6 col-lg-4">
                                    <button className="cus-button primary" onClick={this.handleOnSubmit}>Login</button>
                                    <button className="cus-button " style={{ "marginLeft": "15px" }}> Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>)
    }

}