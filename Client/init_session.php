<?php
session_start();
session_regenerate_id(true);
$_SESSION['user_agent'] = $_SERVER['HTTP_USER_AGENT'];
$_SESSION['logged_in'] = true;

header('Content-Type: application/json');
echo json_encode(['status' => 'success', 'message' => 'Session initialized']);
?>