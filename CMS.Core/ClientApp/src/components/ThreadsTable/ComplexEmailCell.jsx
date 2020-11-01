import React from 'react';

import {Row, Col} from 'antd';

const ComplexEmailCell = (props) => {
    const senderEmail = props.record.senderEmail;
    const senderName = props.record.senderName;

    return(
        <Row>
            {senderName !== "" ? <Col span={24}>{senderName},</Col> : null}
            <Col span={24}>{senderEmail}</Col>
        </Row>
    );
}

export default ComplexEmailCell;