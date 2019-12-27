import React, { Component } from 'react';
import ReactDOM from 'react-dom'
import { Modal } from 'react-bootstrap';
import axios from 'axios';
import validator from 'validator';
import ReactHtmlParser from "react-html-parser";
import '../content/MainPage.css';
const $ = require('jquery');
$.DataTable = require('datatables.net');

export default class DocumentTemplateLIst extends Component {

    constructor(props) {
        super(props);

        this.state = {
            changeTemplateShow: false,
            templateUid: "",
            filename: "",
            uploadedFileName: "No file selected",
            document: "",
            languageCode: "",
            regionUid: "",
            documentTypeId: 0,
            companyId: -1,
            documentTypes: null,
            regions: null,
            languageCodes: null,
            brokerCompanies: null,
            error: {
                filename: false,
                document: false,               
            },
            otherState: {               
                validationMsg: "",
                isValidationShow: 'none',            
            },
        }
        this.bindDocumentTemplateTable = this.bindDocumentTemplateTable.bind(this);
        this.bindDocumentType = this.bindDocumentType.bind(this);
        this.bindRegion = this.bindRegion.bind(this);
        this.bindLanguage = this.bindLanguage.bind(this);
        this.bindComapany = this.bindComapany.bind(this);
        this.viewtemplate = this.viewtemplate.bind(this);
        this.downloadFile = this.downloadFile.bind(this);
        this.openpopup = this.openpopup.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleCloseValidation = this.handleCloseValidation.bind(this);
        this.handleModelHide = this.handleModelHide.bind(this);
    }



    bindDocumentType = () => {
        axios.get("https://localhost:44374/api/Document/BindDocumentTypeDDL")
            .then(result => {
                this.setState({
                    ...this.state,
                    documentTypeId: result.data[0].document_type_id,
                    documentTypes: result.data
                }, () => {
                    this.bindRegion("true")
                    this.bindComapany()

                })
            })
    }

    bindRegion = (isCountrySpecific) => {
        if (isCountrySpecific === "true") {
            $("#regionUid").show();
            $("#regionddldiv").show();
            axios.get("https://localhost:44374/api/Document/BindRegionDDL")
                .then(result => {
                    this.setState({
                        ...this.state,
                        regionUid: result.data[0].value,
                        regions: result.data
                    })
                })
        }
        else {
            this.setState({ ...this.state, regionUid: "", regions: null });
            $("#regionuid").empty();
            $("#regionddldiv").hide();
            $("#regionUid").hide();
        }
    }
    bindLanguage = () => {
        axios.get("https://localhost:44374/api/Document/BindLanguageDDL")
            .then(result => {
                this.setState({
                    ...this.state,
                    languageCode: result.data[0].value,
                    languageCodes: result.data
                })
            })
    }
    bindComapany = () => {
        //axios.get("https://localhost:44374/api/Document/BindBrokerCompaniesDDL")
        //    .then(result => {
        //        this.setState({
        //            ...this.state,
        //            brokerCompanies:result.data
        //        })
        //    })

        this.setState({
            ...this.state,
            brokerCompanies: [{ companyId: -1, companyName: "All Broker Company" }]
        })
    }

    viewtemplate = (templateUid) => {
        axios.get("https://localhost:44374/api/Document/GetTemplateTextByTemplateId", { params: { templateId: templateUid, language: this.state.languageCode, regionUid: this.state.regionUid, product_uid: null, companyId: this.state.companyId } })
            .then(result => {
                //window.open(result.data.TemplateContent, "_blank")
                var newWindow = window.open();
                newWindow.document.write(result.data.TemplateContent);
            })
    }

    downloadFile = (templateUid) => {
        axios.get("https://localhost:44374/api/Document/Download", { params: { templateId: templateUid, language: this.state.languageCode, regionUid: this.state.regionUid, product_uid: null, companyId: this.state.companyId } })
            .then(result => {
                const link = document.createElement('a');
                link.href = window.URL.createObjectURL(new Blob([result.data.templateContent]));
                link.setAttribute('download', result.data.filename);
                document.body.appendChild(link);
                link.click();
                link.remove();
            })
    }

    openpopup = (templateUid, filename) => {
        this.setState({
            ...this.state,
            changeTemplateShow: true,
            templateUid: templateUid,
            filename: filename,
        })
    }

    async bindDocumentTemplateTable() {
        let res = await axios.get("https://localhost:44374/api/Document/BindTemplateList", { params: { language: this.state.languageCode, regionUid: this.state.regionUid, documentTypeId: this.state.documentTypeId, product_uid: null, companyId: this.state.companyId } }, {
            'Content-Type': 'application/json'
        });
        $(this.refs.main).DataTable({
            dom: '<"top"l>rt<"bottom"ip><"clear">',
            data: res.data,
            destroy: true,
            columns: [
                { data: "category_name", title: "Category" },
                { data: "template_name", title: "Template Name" },
                { data: "file_Name", title: "File Name" },
                {
                    title: "View", data: "template_uid",
                    createdCell: (td, cellData, rowData, row, col) => {
                        ReactDOM.render(<a href="javascript:void(0)" onClick={() => { this.viewtemplate(rowData.template_uid) }} >View </a>, td);
                    }
                },
                {
                    title: "Download", data: "template_uid",
                    createdCell: (td, cellData, rowData, row, col) => {
                        ReactDOM.render(<a href="javascript:void(0)" onClick={() => { this.downloadFile(rowData.template_uid) }}>Download </a>, td);
                    }
                },
                {
                    title: "Action", data: "template_uid",
                    createdCell: (td, cellData, rowData, row, col) => {
                        ReactDOM.render(<a href="javascript:void(0)" onClick={() => { this.openpopup(rowData.template_uid, rowData.file_Name) }}>Change Template </a>, td);
                    }
                }
            ],
            autoWidth: false,
            processing: false,
            bSort: true,
            //serverSide: true,
            searching: false,
            paging: true,
            pageLength: 10,
            pagingType: 'numbers',
            order: [[1, "asc"]],
            language: {
                lengthMenu: "Show _MENU_  entries",
                info: "Showing _START_ to _END_ of _TOTAL_ entries",
                emptyTable: "No Record(s) Found",
                paginate: {
                    next: "Next",
                    previous: "Previous"
                }
            },
            info: true,
            emptyTable: "No Record Found",
            lengthMenu: [10, 20, 30, 40, 50],
            columnDefs: [
                { "targets": [3, 4, 5], "orderable": false }
            ],
            initComplete: function (setting, json) {
            },
            drawCallback: function (settings) {
                //hide paging and info when datatable is empty
                var api = this.api();
                if (api.rows({ page: 'current' }).data().length <= 0) {
                    $('#main_info').hide();
                    $('#main_paginate').hide();
                    $('#main_length').hide();
                }
                else {
                    $('#main_info').show();
                    $('#main_paginate').show();
                    $('#main_length').show();
                }
                $('.dataTables_info').addClass('showing-total-entries table-bottom pull-left');
                $('.dataTables_paginate').addClass('table-bottom-links pull-right');
                var pgr = $(settings.nTableWrapper).find('.dataTables_paginate');
                if (settings._iDisplayLength > settings.fnRecordsDisplay()) {
                    pgr.hide();
                } else {
                    pgr.show();
                }

                $('#selectall').on("change", function (e) {
                    $("#main").find('td > input').prop('checked', $(this).prop('checked'));
                });

                $("#main").find('td > input').on("change", function (e) {
                    if (!$(this).is(':checked')) {
                        $('#main').find('th > input:checkbox').prop('checked', false);
                    }
                    else {
                        if ($('#main').find('td >input:checkbox').length === $('#main').find('td >input:checkbox:checked').length) {
                            $('#main').find('th > input:checkbox').prop('checked', true);
                        }
                    }
                });

                $('#main_paginate a').on("click", function (e) {
                    if (!$(this).hasClass('current')) {
                        $('#main').find('input:checkbox').prop('checked', false);
                    }
                });
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $(nRow).attr("id", aData['userId']);
                return nRow;
            }
        });
        // on click event for edit clause
        $('#main tbody').on('click', 'td', function (i, e) {
            if (this.cellIndex !== 0) {
                var userId = $(this).parent().attr("id");
                if (userId !== undefined && userId !== null) {
                    window.location.href = '/userinfo/' + userId;
                }
            }
        });
    }

    componentDidMount() {
        this.bindDocumentType();
        this.bindLanguage()
        setTimeout(() => {
            this.bindDocumentTemplateTable()
        }, 500);
    }

    componentWillUnmount() {
        $('.data-table-wrapper')
            .find('table')
            .DataTable()
            .destroy(true);
    }

    //componentDidUpdate() {
    //    if (this.state.reload) {
    //        this.bindDocumentTemplateTable();
    //        this.setState({
    //            reload: false
    //        });
    //    }
    //}

    handleValidation = () => {   
        let isHtml = (this.state.uploadedFileName === "No file selected") ? true : ((this.state.uploadedFileName.split('.').pop() === "html") ? true : false);
        let validationMsg = "";     
        let newErrorsObj = Object.entries(this.state)
            .filter(([key, value]) => {
                if (this.refs[key] !== undefined && this.refs[key].classList !== undefined && this.refs[key].classList.contains("required")) {
                    if (key === "document") {
                        if (!validator.isEmpty(value) && isHtml)
                            return false;
                        else
                            return true;
                    }
                    else {
                        return validator.isEmpty(value);
                    }
                }
            })
            .reduce((obj, [key, value]) => {
                let msg = this.refs[key].getAttribute("error_msg");  
                if (validationMsg.indexOf('Following') === -1) {
                    validationMsg += '<span style="font-size:16px; font-weight:700;">Following items are required:</span><ul><li>' + ((key === "document" && !isHtml) ? 'Please upload html file' : msg) + '</li>';
                }
                else {
                    validationMsg += '<li>' + ((key === "document" && !isHtml) ? 'Please upload html file' : msg) + '</li>';
                }
                obj[key] = true;
                return obj;
            }, {});

        if (Object.keys(newErrorsObj).length > 0) {  
            this.setState({
                ...this.state,
                error: newErrorsObj,
                otherState: {
                    validationMsg: validationMsg,
                    isValidationShow: 'block'
                }
            });
            return false;
        } else {
            return true;
        }
    }

    handleCloseValidation = () => {
        this.setState({
            ...this.state,
            otherState: {               
                validationMsg: "",
                isValidationShow: 'none'
            }
        });
    }
  

    handleInputChange = async (event) => {
        const target = event.target;
        if (event.target.id === "documentTypeId") {
            await this.setState({
                ...this.state,
                [event.target.id]: event.target.value
            });
            await this.bindRegion(target.options[target.selectedIndex].getAttribute('data-helper')),
                await this.bindComapany()
        }
        else if (event.target.id === "regionUid") {
            this.setState({
                ...this.state,
                [event.target.id]: event.target.value
            }, this.bindComapany());
        }
        else if (event.target.id === "document") {
            var savedTarget = event.target;
            const reader = new FileReader();
            reader.onload = () => {
                this.setState({
                    ...this.state,
                    document: reader.result,
                    uploadedFileName: savedTarget.files[0].name
                });
            }
            reader.readAsText(savedTarget.files[0]);
        }
        else {
            this.setState({
                ...this.state,
                [event.target.id]: event.target.value
            });
        }
    }

    updateDocument = async () => {  
        if (this.handleValidation()) {
            if (this.state.uploadedFileName.split('.').pop() === "html") {
                let jsonData = { "languageCode": this.state.languageCode, "regionUid": this.state.regionUid, "companyId": this.state.companyId, "templateUid": this.state.templateUid, "templateContent": this.state.document, "fileName": this.state.filename }
                await axios.post('https://localhost:44374/api/Document/EditTemplate', jsonData, {
                    'Content-Type': 'application/json'
                }).then(result => {
                    if (result.data) {
                        this.handleModelHide()
                        this.bindDocumentTemplateTable()
                    }
                    else {
                        alert("Error in Template Upload.")
                    }
                })
            }
            else {
                alert("Please upload currect file.")
            }
        }
        
    }

    handleModelHide(e) {
        this.setState({
            ...this.state,
            changeTemplateShow: false,
            templateUid: "",
            filename: "",
            document: "",
            uploadedFileName: "No file selected",
            error: {
                filename: false,
                document: false,
            },
            otherState: {
                validationMsg: "",
                isValidationShow: 'none',
            },
        })
    }


render() {
    return (
        <div className="container-fluid">
            <div className="user-list">
                <div className="page-header">
                    <h2><strong>Manage Document Template</strong></h2>
                </div>
                <div className="button-holder">
                    <div className="main-search-block">
                        <div className="row">
                            <div className="col-md-3 col-lg-2">
                                <div className="form-group">
                                    <label className="col-form-label">Category :</label>
                                    <select className="form-control" id="documentTypeId" ref="documentTypeId" value={this.state.documentTypeId} onChange={this.handleInputChange}>
                                        {this.state.documentTypes !== null ? this.state.documentTypes.map(key => (
                                            <option value={key.document_type_id} key={key.document_type_id} data-helper={key.is_country_specific}>{key.document_type_name}</option>
                                        )) : null}
                                    </select>
                                </div>
                            </div>
                            <div className="col-md-3 col-lg-2" id="regionddldiv">
                                <div className="form-group">
                                    <label className="col-form-label">Region :</label>
                                    <select className="form-control" id="regionUid" ref="regionUid" value={this.state.regionUid} onChange={this.handleInputChange}>
                                        {this.state.regions !== null ? this.state.regions.map(key => (
                                            <option value={key.value} key={key.value}>{key.text}</option>
                                        )) : null}
                                    </select>
                                </div>
                            </div>
                            <div className="col-md-3 col-lg-2">
                                <div className="form-group">
                                    <label className="col-form-label">Language :</label>
                                    <select className="form-control" id="languageCode" ref="languageCode" value={this.state.languageCode} onChange={this.handleInputChange}>
                                        {this.state.languageCodes !== null ? this.state.languageCodes.map(key => (
                                            <option value={key.value} key={key.value} >{key.text}</option>
                                        )) : null}
                                    </select>
                                </div>
                            </div>
                            <div className="col-md-3 col-lg-2">
                                <div className="form-group">
                                    <label className="col-form-label">Broker Company :</label>
                                    <select className="form-control" id="companyId" ref="companyId" value={this.state.companyId} onChange={this.handleInputChange}>
                                        {this.state.brokerCompanies !== null ? this.state.brokerCompanies.map(key => (
                                            <option value={key.companyId} key={key.companyId}>{key.companyName}</option>
                                        )) : null}
                                    </select>
                                </div>
                            </div>
                            <div className="col-md-3 col-lg-2">
                                <div className="form-group">
                                    <button className="cus-button secondary" style={{ "marginTop": "18px", "marginLeft": "15px", "float": "left" }} onClick={() => { this.bindDocumentTemplateTable() }}> Search </button>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div className="table-resposive" style={{ "marginTop": "50px" }}>
                    <table ref="main" id="main" className="darkgrid table table-bordered table-hover">
                    </table>
                    <Modal show={this.state.changeTemplateShow} size="lg" aria-labelledby="contained-modal-title-vcenter" centered="true" onHide={this.handleModelHide}>
                        <Modal.Header closeButton>
                            <Modal.Title>Change Document Template</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div className="container-fluid">
                                <div className="alert alert-danger alert-dismissable  fade in" style={{ display: this.state.otherState.isValidationShow }}>
                                    <a className="close" data-dismiss="alert" aria-label="close" onClick={this.handleCloseValidation}>×</a>
                                    <div id="failMessageEdit">{ReactHtmlParser(this.state.otherState.validationMsg)} </div>
                                </div>
                                <div className="user-list">
                                    <div className="row">
                                        <div className="col-lg-12">
                                            <div className="form-group">
                                                <label >File Name:</label>
                                                <input type="text" className={this.state.error.filename ? "input-validation-error form-control required" : "form-control required"} id="filename" ref="filename" value={this.state.filename} onChange={this.handleInputChange} error_msg="File Name" />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-xs-12 botpad25">
                                            <label>Document</label>
                                            <div className="file-upload cus-filecontrol">
                                                <div className={this.state.error.document ? "input-validation-error file-select" : "file-select"}>
                                                    <div className="file-select-button" id="fileName">CHOOSE FILE</div>
                                                    <div className="file-select-name" ref="noFile">{this.state.uploadedFileName}</div>
                                                    <input type="file" className="required" name="1" id="document" ref="document" onChange={this.handleInputChange} error_msg="Document File" accept=".html" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <input type="button" className="cus-button primary" value="Upload" onClick={() => this.updateDocument()} />
                            <input type="button" className="cus-button secondary" style={{ 'marginLeft': '10px' }} value="Cancel" onClick={() => this.handleModelHide()} />
                        </Modal.Footer>
                    </Modal>

                </div>
            </div>
        </div>
    )
}
}