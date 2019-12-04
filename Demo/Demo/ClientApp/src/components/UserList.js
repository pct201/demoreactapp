import React, { Component } from 'react';
import axios from 'axios';

const $ = require('jquery');
$.DataTable = require('datatables.net');

export default class UserList extends Component {

    async componentDidMount() {
        let res = await axios.get("http://192.168.2.44/Api/Employee/AllEmployeeDetails");
        $(this.refs.main).DataTable({
            dom: '<"top"l>rt<"bottom"ip><"clear">',
            data: res.data,
            columns: [
                { data: "userId", title: "UserId", className: "display-none" },
                { data: "email", title: "Email Address" },
                { data: "first_name", title: "First Name" },
                { data: "last_name", title: "Last Name" },
                { data: "mobile_Number", title: "Contact Number" },
                { data: "education_Name", title: "Education" },
                { data: "salary", title: "salary" },
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
            order: [[0, "asc"]],
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
                { "targets": [0, 2, 3, 4], "orderable": false }
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
                    $('#glexchangebal-setting-table_length').show();
                }
                $('.dataTables_info').addClass('showing-total-entries table-bottom pull-left');
                $('.dataTables_paginate').addClass('table-bottom-links pull-right');
                var pgr = $(settings.nTableWrapper).find('.dataTables_paginate');
                if (settings._iDisplayLength > settings.fnRecordsDisplay()) {
                    pgr.hide();
                } else {
                    pgr.show();
                }
            }
        });
    }

    componentWillUnmount() {
        $('.data-table-wrapper')
            .find('table')
            .DataTable()
            .destroy(true);
    }

    shouldComponentUpdate() {
        return false;
    }

    render() {
        return (
            <div className="table-resposive">
                <table ref="main" id="main" className="darkgrid table table-bordered table-hover">
                </table>
            </div>
        )
    }
}