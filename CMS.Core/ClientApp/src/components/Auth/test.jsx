    
    import { createContext } from ("react");

    const App = (props) => {
        const ThemeContext = createContext('light')
        return(
            <ThemeContext.Provider value="dark">
                <MainContainer />
            </ThemeContext.Provider>
        )
    }

    
      
    









