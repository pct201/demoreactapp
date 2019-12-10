import React, { Component } from 'react';
import UserList from './UserList';
import '../content/MainPage.css';

export default class MainPage extends Component {
    render() {
        return (
            <div className="container">
                <div className="user-list">
                    <div className="page-header">
                        <center><h2>Manage Users</h2></center>
                    </div>
                    <UserList />
                </div>
            </div>
            )
    }
}