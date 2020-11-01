import React from 'react';
import {Link} from 'react-router-dom';

import {Row, Col} from 'antd';

 
const NormalLayoutSubjectCell = (props) => {
    const threadUrl = `/threads/${props.record.id}`;
    
    return(
        <Link to={threadUrl}>
            <Row>
                <Col span={24}><b>{props.record.title}</b></Col>
            </Row>
        </Link>  
    );
}

export default NormalLayoutSubjectCell;