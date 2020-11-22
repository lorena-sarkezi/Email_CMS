import React from 'react';
import {Link} from 'react-router-dom';

const AlreadyLoggedIn = props => 
    <div className='already-logged-in'>
        You are already logged in<br/>
        <Link to="/">Go to Index Page</Link>
    </div>

export default AlreadyLoggedIn;