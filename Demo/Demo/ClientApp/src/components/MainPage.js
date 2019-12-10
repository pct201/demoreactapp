import React, { Component } from 'react';
import UserList from './UserList';
import '../content/MainPage.css';

export default class MainPage extends Component {
    render() {
        return (
            <div className="container-fluid">
                <div className="user-list">
                    <div className="page-header">
                        <h2><strong>Manage Users</strong></h2>
                    </div>
                    <UserList />
                </div>
            </div>
        )
    }
}