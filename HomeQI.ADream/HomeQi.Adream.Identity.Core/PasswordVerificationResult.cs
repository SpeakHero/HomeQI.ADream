// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Specifies the results for password verification.
    /// </summary>
    public enum PasswordVerificationResult
    {
        /// <summary>
        /// ָʾ������֤ʧ�ܡ�
        /// </summary>
        Failed = 0,

        /// <summary>
        /// ָʾ������֤�ɹ���
        /// </summary>
        Success = 1,

        /// <summary>
        ///ָʾ������֤�ǳɹ��ģ�����������ʹ�ò��Ƽ����㷨���б���ġ�
        /// and should be rehashed and updated.
        /// </summary>
        SuccessRehashNeeded = 2
    }
}