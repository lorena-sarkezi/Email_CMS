import React, { useEffect, useState } from 'react';
import {Table} from 'antd';
import axios from 'axios';

import SmallLayoutSubjectCell from './SmallLayoutSubjectCell';
import NormalLayoutSubjectCell from './NormalLayoutSubjectCell';
import ComplexEmailCell from './ComplexEmailCell';
import DeleteThreadButton from './DeleteThreadButton';


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
            dataIndex: "title",
            key: new Date(),
            responsive:["xs"],
            render: (text, row) => <SmallLayoutSubjectCell record={row} />
        },
        {
            title: 'Subject',
            dataIndex: 'title',
            key:'subject',
            responsive:["sm"],
            render: (text, row) => <NormalLayoutSubjectCell record={row} />
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
            dataIndex:"latestMessageTimestamp",
            key:"latestMessageTimestamp"
        },
        {
            title:"",
            key:"latestMessageTimestamp",
            render: (text,row) => <DeleteThreadButton />
            
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