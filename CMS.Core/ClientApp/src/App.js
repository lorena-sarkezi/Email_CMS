import React, {useState, useEffect} from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';

import ThreadsTable from './components/ThreadsTable';
import MessagesDisplayContainer from './components/MessagesDisplayContainer';
import Login from './components/Login';
import AuthorizedRoute from './components/Auth/AuthorizedRoute';
import Header from './components/Header';
import Register from './components/Register';
import NotFound from './components/Misc/NotFound';

import {AuthContext} from './components/Auth/AuthContext';

import { Row, Col, Layout} from 'antd';

import './style.css';
import 'antd/dist/antd.css';

export default function App() {
  
  const [globalAuthState, setGlobalAuthState] = useState(false);

  const userContextVal = {
    value: globalAuthState,
    update: value => setGlobalAuthState(value)
  };


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
    <AuthContext.Provider value={userContextVal}>
      <Layout.Header style={{padding: "0px"}}>
        <Row>
          <Col {...sideRowProps}></Col>
          <Col {...mainRowProps}>
           <Header />
            
          </Col>
          <Col {...sideRowProps}></Col>
        </Row>


      </Layout.Header>



      <Layout.Content>

        <Row>
          <Col {...sideRowProps}></Col>
          <Col {...mainRowProps}>
            <Switch>




              <AuthorizedRoute path="/" exact component={ThreadsTable} />

              <Route path="/threads/:threadId" component={MessagesDisplayContainer} />

              <Route path="/login" component={Login} />

              <Route path="/register" component={Register} />

              <Route path="/notfound" component={NotFound} />

              <Route path="*" component={() => <Redirect to="/notfound" />} />

            </Switch>

          </Col>
          <Col {...sideRowProps}></Col>
        </Row>
      </Layout.Content>

    </AuthContext.Provider>

  );
}
