import React from 'react';
import { Switch, Route, } from 'react-router-dom';

import ThreadsTable from './components/ThreadsTable';
import MessagesDisplayContainer from './components/MessagesDisplayContainer';

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
      <Layout.Header></Layout.Header>
      <Layout.Content>


        <Row>
          <Col {...sideRowProps}></Col>
          <Col {...mainRowProps}>
            <Switch>
              <Route path="/" exact >
                <ThreadsTable />
              </Route>

              <Route path="/threads/:threadId">
                <MessagesDisplayContainer />
              </Route>
              
            </Switch>

          </Col>
          <Col {...sideRowProps}></Col>
        </Row>
      </Layout.Content>

    </>

  );
}
