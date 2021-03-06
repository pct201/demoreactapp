﻿import React from 'react';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export default props => (
    <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
            <Navbar.Brand>
                <span>React Demo App</span>
            </Navbar.Brand>
            <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
            <Nav>
                <LinkContainer to={'/'} exact>
                    <NavItem>
                        <Glyphicon glyph='home' /> Manage User
          </NavItem>
                </LinkContainer>
            </Nav>
            <Nav>
                <LinkContainer to={'/Login'} exact>
                    <NavItem>
                        <Glyphicon glyph='home' /> Login
          </NavItem>
                </LinkContainer>
            </Nav>
            <Nav>
                <LinkContainer to={'/Document'} exact>
                    <NavItem>
                        <Glyphicon glyph='file' /> Document Template
          </NavItem>
                </LinkContainer>
            </Nav>
        </Navbar.Collapse>
    </Navbar>
);
