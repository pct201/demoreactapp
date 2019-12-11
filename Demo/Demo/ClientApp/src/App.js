import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import MainPage from './components/MainPage';
import UserInfo from './components/UserInfo';

require('dotenv').config()

export default () => (
    <Layout>
        <Route exact path={'/'} component={MainPage} />
        <Route exact path={'/UserInfo/:id?'} component={UserInfo} />
    </Layout>
);
