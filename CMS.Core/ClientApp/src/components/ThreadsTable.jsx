import React, { useEffect, useState } from 'react';
import {Table, Row, Col, Button, Popconfirm} from 'antd';
import DeleteOutlined from '@ant-design/icons/DeleteOutlined';
import axios from 'axios';

const ComplexSubjectCell = (props) => {
    const senderName = props.record.senderName;
    const senderEmail = props.record.senderEmail;

    return(
        <Row>
            <Col span={24}><b>{props.record.subject}</b></Col>
            <Col span={24}>{senderName !== "" ? senderName : senderEmail}</Col>
        </Row>
    );
}

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

const DeleteButton = (props) => {
    return(
        <Popconfirm
            title="Are you sure?"
            okText="Yes"
            cancelText="No"
        >
            <Button type="danger" icon={<DeleteOutlined />} />
        </Popconfirm>
    )
}

export default function ThreadsTable(props){

    const [threads, setThreads] = useState(null);

    const getInitialThreads = async () =>{
        const response = await axios.get("/api/v1/messaging/threads")
        
        console.log(response.data);
        setThreads(response.data);
    }

    useEffect(() =>{
        getInitialThreads();
    }, []);

    const tableColumns = [
        {
            title:"Subject",
            dataIndex: "subject",
            key: new Date(),
            responsive:["xs"],
            render: (text, row) => <ComplexSubjectCell record={row} />
        },
        {
            title: 'Subject',
            dataIndex: 'subject',
            key:'subject',
            responsive:["sm"]
        },
        {
            title:"Initial sender",
            dataIndex: "senderEmail",
            key:Math.random(),
            responsive:["sm"],
            render: (text, row) => <ComplexEmailCell record={row} />
        },
        {
            title:"Latest message",
            dataIndex:"timestamp",
            key:"timestamp"
        },
        {
            title:"",
            key:"timestamp",
            render: (text,row) => <DeleteButton />
            
        }

    ]

    const tableProps = {
        dataSource: threads,
        columns: tableColumns
    };

    return(
        <Table {...tableProps}/>
    )
}