import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import UserInfo from './components/UserInfo';
import UserList from './components/UserList'; 
import Login from './components/Login';
import DocumentTemplateLIst from './components/DocumentTemplateLIst';
require('dotenv').config()

export default () => (
    <Layout>
        <Route exact path={'/'} component={UserList} />
        <Route exact path={'/UserInfo/:id?'} component={UserInfo} />
        <Route exact path={'/Login'} component={Login} />
        <Route exact path={'/Document'} component={DocumentTemplateLIst} />        
    </Layout>
);
