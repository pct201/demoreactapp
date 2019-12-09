import React, { Component } from 'react';
import axios from 'axios';
import DatePicker from "react-16-bootstrap-date-picker";
import Summernote from './Summernote';
import UploadImage from './UploadImage';
import { connect } from 'react-redux';
import validator from 'validator';

class UserInfo extends Component {
    state = {
        mainState: {
            userId: 0,
            first_name: "",
            last_name: "",
            email: "",
            mobile_number: "",
            education_id: "",
            salary: "",
            is_married: false,
            address: "",
            document: "",
            blog: "",
            profile_picture: "",
            birth_date: new Date().toISOString()
        },
        error: {
            first_name: false,
            last_name: false,
            email: false,
            mobile_number: false
        },
        otherState: {
            fileName: "No file selected",
            isDeleteShow: false,
            educationData: null
        }
    }

    componentDidUpdate(nextProps) {
        if (nextProps.summernotedata !== this.props.summernotedata || nextProps.cropperdata !== this.props.cropperdata) {
            this.setState({
                mainState: {
                    ...this.state.mainState,
                    blog: this.props.summernotedata,
                    profile_picture: this.props.cropperdata
                }
            });
        }
    }

    componentDidMount = () => {

        axios.get("http://192.168.2.44/Api/Employee/GetEducationList")
            .then(result => {
                this.setState({
                    otherState: {
                        ...this.state.otherState,
                        educationData: result.data
                    }
                })
            })

        let userId = this.props.match.params.id;
        if (userId > 0) {
            axios.get("http://192.168.2.44/Api/Employee/GetEmployeeDetailsById/" + userId)
                .then(result => {
                    this.setState({
                        mainState: {
                            userId: result.data.userId,
                            first_name: result.data.first_name,
                            last_name: result.data.last_name,
                            email: result.data.email,
                            mobile_number: result.data.mobile_number,
                            education_id: result.data.education_Id,
                            salary: result.data.salary,
                            is_married: result.data.is_Married,
                            address: result.data.address,
                            document: result.data.document,
                            blog: result.data.blog,
                            profile_picture: result.data.profile_picture,
                            birth_date: result.data.birth_Date
                        }
                    })
                })
        }
    }

    uploadFile = (event) => {
        this.setState({
            otherState: {
                ...this.state.otherState,
                fileName: event.target.files[0].name,
                isDeleteShow: true
            }
        });
    }

    cancelUpload = () => {
        this.setState({
            mainState: {
                ...this.state.mainState,
                document: null
            },
            otherState: {
                ...this.state.otherState,
                fileName: "No file selected",
                isDeleteShow: false
            }
        });
    }

    handleDatepickerChange = (value, formattedValue) => {
        this.setState({
            mainState: {
                ...this.state.mainState,
                birth_date: value,
                formattedValue: formattedValue
            }
        });
    }

    handleInputChange = event => {
        if (event.target.id === "document") {
            var savedTarget = event.target;
            const reader = new FileReader();
            reader.onload = () => {
                this.setState({
                    mainState: {
                        ...this.state.mainState,
                        [savedTarget.id]: reader.result
                    },
                    otherState: {
                        ...this.state.otherState,
                        fileName: savedTarget.files[0].name,
                        isDeleteShow: true
                    }
                });
            }
            reader.readAsDataURL(savedTarget.files[0]);
        }
        else if (event.target.id === "is_married") {
            this.setState({
                mainState: {
                    ...this.state.mainState,
                    [event.target.id]: event.target.checked
                }
            });
        }
        else {
            this.setState({
                mainState: {
                    ...this.state.mainState,
                    [event.target.id]: event.target.value
                }
            });
        }
    }
    handleValidation = () => {
        const { mainState } = this.state;
        //create new errors object
        let newErrorsObj = Object.entries(mainState)
            .filter(([key, value]) => {
                if (this.refs[key] !== undefined && this.refs[key].classList !== undefined && this.refs[key].classList.contains("required")) {
                    if (this.refs[key].hasAttribute("additional_validation")) {
                        if (this.refs[key].attributes.additional_validation.value === "email") {
                            return !validator.isEmail(value);
                        }
                        else if (this.refs[key].attributes.additional_validation.value === "mobile_number") {
                            return !validator.isMobilePhone(value, 'en-IN');
                        }
                    }
                    else {
                        return validator.isEmpty(value);
                    }
                }
            })
            .reduce((obj, [key, value]) => {
                obj[key] = true;
                return obj;
            }, {});

        if (Object.keys(newErrorsObj).length > 0) {
            this.setState({
                error: newErrorsObj
            });
            return false;
        } else {
            return true;
        }
    }

    handleOnSubmit = event => {
        if (this.handleValidation()) {
            alert("success")
            axios.post('https://localhost:44374/api/Employee/InsertEmployeeDetails', this.state.mainState, {
                'Content-Type': 'application/json'
            })
                .then(
                    this.props.history.push('/', null)
                )
        }
    };

    render() {
        const { error } = this.state;
        let deleteStyle = {
            display: this.state.otherState.isDeleteShow ? "block" : "none"
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
                                    <input type="text" className={error.first_name ? "input-validation-error form-control required" : "required form-control"} id="first_name" ref="first_name" value={this.state.mainState.first_name} onChange={this.handleInputChange} />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Last Name :</label>
                                <div className="col-md-9">
                                    <input type="text" className={error.last_name ? "input-validation-error form-control required" : "required form-control"} id="last_name" ref="last_name" value={this.state.mainState.last_name} onChange={this.handleInputChange} />
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
                                    <input type="text" className={error.email ? "input-validation-error form-control required" : "required form-control"} additional_validation="email" id="email" ref="email" value={this.state.mainState.email} onChange={this.handleInputChange} />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="form-group row">
                                <label className="col-md-3 col-form-label">Mobile No. :</label>
                                <div className="col-md-9">
                                    <input type="phone" className={error.mobile_number ? "input-validation-error form-control required" : "required form-control"} additional_validation="mobile_number" id="mobile_number" ref="mobile_number" value={this.state.mainState.mobile_number} onChange={this.handleInputChange} />
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
                                    <select className="form-control" id="education_id" ref="education_id" value={this.state.mainState.education_id} onChange={this.handleInputChange}>
                                        <option>Select</option>
                                        {this.state.otherState.educationData !== null ? this.state.otherState.educationData.map(key => (
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
                                    <input type="text" className="form-control" id="salary" ref="salary" value={this.state.mainState.salary} onChange={this.handleInputChange} />
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
                                    <DatePicker id="birth_date" value={this.state.mainState.birth_date} onChange={this.handleDatepickerChange} dateFormat="YYYY-MM-DD" showClearButton={false} disableEntry={true} />
                                </div>
                            </div>
                        </div>

                        <div className="col-md-6">
                            <div className="mvc-checkbox" style={{ 'paddingTop': '4px' }}>
                                <div className="toggle-switch ipe-switch">
                                    <label className="default-label" style={{ paddingRight: '10px !important', fontWeight: '700' }}>Married</label>
                                    <label className="switch" style={{ display: 'inline-block', 'verticalAlign': '-14px' }}>
                                        <input type="checkbox" checked={this.state.mainState.is_married} className="togBtn" id="is_married" ref="is_married" onChange={this.handleInputChange} />
                                        <div className="slider round" ></div>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div className="form-group">
                        <label >Address:</label>
                        <textarea className="form-control" rows="5" id="address" ref="address" value={this.state.mainState.address} onChange={this.handleInputChange}></textarea>
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
                                    <div className="file-select-name" ref="noFile">{this.state.otherState.fileName}</div>
                                    <div className="file-select-delete register-delete" style={deleteStyle} onClick={this.cancelUpload}>x</div>
                                    <input type="file" name="1" id="document" ref="document" onChange={this.handleInputChange} />
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
                        <input type="button" className="btn btn-primary" value="Submit" style={{ "marginRight": "1%" }} onClick={this.handleOnSubmit} />
                        <input type="button" className="btn btn-secondary" value="Cancel" />
                    </div>
                    <br />
                </div>
            </div>
        )
    }
}

function mapStatetoProps(state) {
    return {
        summernotedata: state.summernote.summernoteContent,
        cropperdata: state.uploadImage.cropResult
    }
}

export default connect(mapStatetoProps, null)(UserInfo)
