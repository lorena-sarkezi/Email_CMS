import React from 'react';
import { Switch, Route, } from 'react-router-dom';

import ThreadsTable from './components/ThreadsTable';
import MessagesDisplayContainer from './components/MessagesDisplayContainer';
import Login from './components/Login';
import AuthorizedRoute from './components/Auth/AuthorizedRoute';

import { Row, Col, Layout } from 'antd';

import './style.css';
import 'antd/dist/antd.css';

export default function App() {

  const sideRowProps = {
    xs: 0,
    sm: 2,
    md: 3,
    lg: 6
  };

  const mainRowProps = {
    xs: 24,
    sm: 20,
    md: 18,
    lg: 12
  };

  return (
    <>
      <Layout.Header>
        <Row>
          <Col{...sideRowProps}></Col>
          <Col {...mainRowProps}>
            <h1 className="header-text">
              EMAIL CMS
            </h1>
          </Col>
          <Col{...sideRowProps}></Col>
        </Row>


      </Layout.Header>
      <Layout.Content>

        <Row>
          <Col {...sideRowProps}></Col>
          <Col {...mainRowProps}>
            <Switch>
              {/* <AuthorizedRoute path="/" exact >
                <ThreadsTable />
              </AuthorizedRoute> */}

              <AuthorizedRoute path="/" exact component={ThreadsTable} />

              <AuthorizedRoute path="/threads/:threadId" component={MessagesDisplayContainer} />

              <Route path="/login" component={Login} />

            </Switch>

          </Col>
          <Col {...sideRowProps}></Col>
        </Row>
      </Layout.Content>

    </>

  );
}
