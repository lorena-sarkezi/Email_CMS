import React from 'react';
import {Link} from 'react-router-dom';

import {Row, Col} from 'antd';
 
const SmallLayoutSubjectCell = (props) => {
    const senderName = props.record.senderName;
    const senderEmail = props.record.senderEmail;
    const threadUrl = `/threads/${props.record.id}`;

    return(
        <Link to={threadUrl}>
            <Row>
                <Col span={24}><b>{props.record.title}</b></Col>
                <Col span={24}>By: {senderName !== "" ? senderName : senderEmail}</Col>
            </Row>
        </Link>  
    );
}

export default SmallLayoutSubjectCell;