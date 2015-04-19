// AppServer test host (https://github.com/mkloubert/AppServer)
// Copyright (c) Marcel Joachim Kloubert, All rights reserved.
//
// This application is part of the AppServer project (https://github.com/mkloubert/AppServer):
// you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// It is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with that plugin.  If not, see <http://www.gnu.org/licenses/>.

open MarcelJoachimKloubert.AppServer
open System

[<EntryPoint>]
let main argv =
    let mutable result = 0

    try
        use server = new ApplicationServer()

        server.Started.Add(fun e ->
            Console.WriteLine("Server has been started."))
        server.Starting.Add(fun e ->
            Console.WriteLine("Server is starting..."))

        server.Start()

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("===== ENTER =====")

        Console.ReadLine() |> ignore
    with
    | :? Exception as e ->
        result <- 1

    result