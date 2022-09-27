<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ page import="java.sql.*" %>

<% request.setCharacterEncoding("utf-8");%>

<% 
    String countStr = request.getParameter("count");

    int count = countStr == null ? 8 : Integer.parseInt(countStr);
    count = count == 0 ? 8 : count;

    Connection conn=null;
    PreparedStatement pstmt = null;
    ResultSet rs = null;

    String str="";

    try {
        String jdbcUrl = "jdbc:mysql://localhost:3306/poe";
        String userID = "poeadmin";
        String userPW = "poe1admin2";

        Class.forName("com.mysql.jdbc.Driver");
        conn = DriverManager.getConnection(jdbcUrl, userId, userPW);

        //랜덤 방식 먼저 적용
        String sql= "select * from characters order by rand() limit ?";
        pstmt=conn.prepareStatement(sql);
        pstmt.setInt(1, count);
        rs = pstmt.execute();

        JSONArray characters = new JSONArray();
        while (rs.next()) {
            JSONObject temp = new JSONObject();

            String name = rs.getString("name");
            float baseHP = rs.getFloat("baseHP");
            float baseATK = rs.getFloat("baseATK");
            float baseDEF = rs.getFloat("baseDEF");
            float baseSPD = rs.getFloat("baseSPD");

            String[] skillList = rs.getString("skillNoList");
            int[] skillNoList = new int[skillList.length()];

            for (int i=0; i<skillList.length(); i++) {
                skillNoList[i] = Integer.parseInt(skillList[i]);
            }

            temp.put("name", name);
            temp.put("baseHP", baseHP);
            temp.put("baseATK", baseATK);
            temp.put("baseDEF", baseDEF);
            temp.put("baseSPD", baseSPD);
            temp.put("skillNoList", skillNoList);
            characters.add(temp);
        }

        out.print(characters);
    }catch(Exception e){ 
        e.printStackTrace();
        out.println("error");
    }finally{
		if(rs != null) try {rs.close();} catch(SQLException sqle){}
		if(pstmt != null) try {pstmt.close();} catch(SQLException sqle){}
		if(conn != null) try {conn.close();} catch(SQLException sqle){} 
    }
%>