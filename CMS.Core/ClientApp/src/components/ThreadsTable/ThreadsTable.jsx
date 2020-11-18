import React, { useEffect, useState } from 'react';
import {Table, Select, Row, Col} from 'antd';
import axios from '../../axios';

import SmallLayoutSubjectCell from './SmallLayoutSubjectCell';
import NormalLayoutSubjectCell from './NormalLayoutSubjectCell';
import ComplexEmailCell from './ComplexEmailCell';
import DeleteThreadButton from './DeleteThreadButton';
import TablePagination from './TablePagination';

const {Option} = Select;

const defaultPagination = {
    current: 1,
    total: 10,
    pageSize: 10
}

export default function ThreadsTable(props){

    const [threads, setThreads] = useState(null);
    const [isTableLoading, setIsTableLoading] = useState(false);
    const [pagination, setPagination] = useState(defaultPagination);

    const handleSetPageSize = value => {
        let current = {...pagination};
        current.pageSize = value;
        
        setPagination(current);
    }

    const getPageCount = async () =>{
        const response = await axios.get("/api/v1/messaging/threads/count")
        
        let data = {...pagination};
        data.total = response.data;

        setPagination(data);
    }

    const getThreadsPaged = async () =>{
        setIsTableLoading(true);

        const requestParams ={
            count:pagination.pageSize,
            start: pagination.current * pagination.pageSize,
            end: pagination.current * pagination.pageSize + pagination.pageSize
        }

        const response = await axios.get("/api/v1/messaging/threads", requestParams)
        
        //console.log(response.data);
        setThreads(response.data);
        setIsTableLoading(false);
    }

    useEffect(()=>{
        getThreadsPaged();
    }, [pagination])

    useEffect(() =>{
        getPageCount();
        getThreadsPaged();
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
        columns: tableColumns,
        loading: isTableLoading,
        pagination: <TablePagination pageState={pagination} setPageState={setPagination} />
    };

    return(
        <>
            <Row>
                <Col>
                    <Select onChange={value => handleSetPageSize(value)}>
                        <Option value={10} >10</Option>
                        <Option value={20}>20</Option>
                        <Option value={30}>30</Option>
                    </Select>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Table {...tableProps}/>
                </Col>
            </Row>
        </>
        
        
    )
}