import React, { Component } from 'react';
import axios from 'axios';
import DatePicker from "react-16-bootstrap-date-picker";
import Summernote from './Summernote';
import UploadImage from './UploadImage';


export default class UserInfo extends Component {

    state = {
        fileName: "No file selected",
        isDeleteShow: false,
        birthDate: new Date().toISOString(),
        educationData: null,
        educationValue: null
    }

    componentDidMount = () => {
        axios.get("http://192.168.2.44/Api/Employee/GetEducationList")
            .then(result => {
                this.setState({
                    educationData: result.data
                })
            })
    }
    handleEducationChnage = (event) => {
        this.setState({
            educationValue: event.target.value
        })
    }

    uploadFile = (event) => {
        this.setState({
            fileName: event.target.files[0].name,
            isDeleteShow: true
        });
    }

    cancelUpload = () => {
        this.setState({
            fileName: "No file selected",
            isDeleteShow: false
        });
    }

    handleDatepickerChange = (value, formattedValue) => {
        this.setState({
            birthDate: value,
            formattedValue: formattedValue
        });
    }

    render() {
        let deleteStyle = {
            display: this.state.isDeleteShow ? "block" : "none"
        }
        return (
            <div className="user-info">
                <div className="page-header">
                    <center><h2>User Information</h2></center>
                </div>
                <div className="main-content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">First Name :</label>
                                <div className="col-md-9">
                                    <input type="text" className="form-control" id="name" ref="name" />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Last Name :</label>
                                <div className="col-md-9">
                                    <input type="text" className="form-control" id="name" ref="name" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="row">
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Email :</label>
                                <div className="col-md-9">
                                    <input type="text" className="form-control" id="name" ref="name" />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Mobile No. :</label>
                                <div className="col-md-9">
                                    <input type="text" className="form-control" id="name" ref="name" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="row">
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Education :</label>
                                <div className="col-md-9">
                                    <select className="form-control" id="ddEducation" onChange={this.handleEducationChnage}>
                                        <option defaultValue>Select</option>
                                        {this.state.educationData != null ? this.state.educationData.map(key => (
                                            <option value={key.education_Id} key={key.education_Id}>{key.education_Name}</option>
                                        )) : null}
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Salary :</label>
                                <div className="col-md-9">
                                    <input type="text" className="form-control" id="name" ref="name" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="row">
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Birth Date :</label>
                                <div className="col-md-9">
                                    <DatePicker value={this.state.birthDate} onChange={this.handleDatepickerChange} dateFormat="YYYY-MM-DD" showClearButton={false} disableEntry={true} />
                                </div>
                            </div>
                        </div>

                        <div className="col-md-6">
                            <div className="mvc-checkbox" style={{ 'paddingTop': '4px' }}>
                                <div className="toggle-switch">
                                    <label className="default-label" style={{ paddingRight: '10px !important' }}>Married</label>
                                    <label className="switch" style={{ display: 'inline-block', 'verticalAlign': '-14px' }}>
                                        <input type="checkbox" defaultChecked="true" className="togBtn" />
                                        <div className="slider round"></div>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="form-group">
                        <label >Address:</label>
                        <textarea className="form-control" rows="5" id="comment"></textarea>
                    </div>
                    <br />
                    <div className="form-group">
                        <Summernote />
                    </div>
                    <br />
                    <UploadImage />
                    <br />
                    <div className="row">
                        <div className="col-xs-6 botpad25">
                            <label>Document</label>
                            <div className="file-upload cus-filecontrol">
                                <div className="file-select">
                                    <div className="file-select-button" id="fileName">CHOOSE FILE</div>
                                    <div className="file-select-name" ref="noFile">{this.state.fileName}</div>
                                    <div className="file-select-delete register-delete" style={deleteStyle} onClick={this.cancelUpload}>x</div>
                                    <input type="file" name="1" id="CNC-question-23" onChange={this.uploadFile} />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="termsAndCond">
                        <label>
                            <input type="checkbox" className="form-check-input" value="" />
                            <p>I Accept terms and conditions</p>
                        </label>
                    </div>
                    <br />
                    <div>
                        <input type="button" className="btn btn-primary" value="Submit" style={{ "marginRight": "1%" }} />
                        <input type="button" className="btn btn-secondary" value="Cancel" />
                    </div>
                    <br />
                </div>
            </div>
        )
    }
}