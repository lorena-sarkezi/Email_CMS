import React from 'react';

import ThreadsTable from './components/ThreadsTable';

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
            <ThreadsTable />
          </Col>
          <Col {...sideRowProps}></Col>
        </Row>
      </Layout.Content>

    </>

  );
}
