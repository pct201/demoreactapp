import React, { Component } from 'react';
import ReactDOM from 'react-dom'
import axios from 'axios';

const $ = require('jquery');
$.DataTable = require('datatables.net');

export default class UserList extends Component {

    constructor(props) {
        super(props);
        this.state = {
            reload: false
        }
        this.deleteUser = this.deleteUser.bind(this);
    }

    async componentDidMount() {
        let res = await axios.get("http://192.168.2.44/Api/Employee/AllEmployeeDetails");
        $(this.refs.main).DataTable({
            dom: '<"top"l>rt<"bottom"ip><"clear">',
            data: res.data,
            columns: [
                {
                    title: "<input type='checkbox' className='togBtn' id='selectall' ref='selectall' name='selectall'>", data: "userId",
                    //render: function (data, type, row) {
                    //    return "<input type='checkbox' class='togBtn' onChange=\"selectCheckbox(this);\" id= " + row.userId + ">";
                    //}
                    createdCell: (td, cellData, rowData, row, col) => {
                        ReactDOM.render(<input type='checkbox' className='togBtn' id={rowData.userId} />, td);
                    }
                },
                { data: "first_name", title: "First Name" },
                { data: "last_name", title: "Last Name" },
                { data: "email", title: "Email Address" },
                { data: "mobile_Number", title: "Contact Number" },
                { data: "education_Name", title: "Education" },
                { data: "salary", title: "Salary" },
                { data: "birth_Date", title: "Birth Date" },
                {
                    data: "is_Married", title: "Married", render: function (data, type, row) {
                        if (row.isMarried) return 'Yes';
                        else return 'No';
                    }
                },
                { data: "address", title: "Address" },
                { data: "updated_date", title: "Updated On" }
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
                { "targets": [0, 4,6,7,10], "orderable": false }
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
                        if ($('#main').find('td >input:checkbox').length == $('#main').find('td >input:checkbox:checked').length) {
                            $('#main').find('th > input:checkbox').prop('checked', true);
                        }
                    }
                });
            }
        });
    }

    componentWillUnmount() {
        $('.data-table-wrapper')
            .find('table')
            .DataTable()
            .destroy(true);
    }

     deleteUser(e) {
        var table = this.refs.main;
        var inputs = table.querySelectorAll("td input[type='checkbox']");
        let userIds = [];
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                userIds = [...userIds, inputs[i].id];
            }
        }
        let res =  axios.delete("http://192.168.2.44/Api/Employee/DeleteEmployee?" + userIds.toString());
        if (res.status == 200) {
            this.setState({
                reload: true
            });
        }
    }

    render() {
        return (
            <div>
                <div className="button-holder">
                    <button className="cus-button primary" style={{ "marginLeft": "15px", "float": "right" }} onClick={this.deleteUser} >Delete</button>
                </div>
                <div className="table-resposive">
                    <table ref="main" id="main" className="darkgrid table table-bordered table-hover">
                    </table>
                </div>
            </div>
        )
    }
}