import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import UserInfo from './components/UserInfo';
import UserList from './components/UserList';

require('dotenv').config()

export default () => (
    <Layout>
        <Route exact path={'/'} component={UserList} />
        <Route exact path={'/UserInfo/:id?'} component={UserInfo} />
    </Layout>
);
