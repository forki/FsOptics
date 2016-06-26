﻿namespace FsOptics

type Update
type Update<'a>

type Optic<'s,'a,'b,'t> = (Update -> 'a -> Update<'b>) -> (Update -> 's -> Update<'t>)
type Optic<'s, 'a> = Optic<'s, 'a, 'a, 's>

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Update =
  val result: 'a -> Update<'a>
  val inline (</>): Update<'a> -> ('a -> 'b) -> Update<'b>
  val (<&>):       ('a -> 'b)  -> Update<'a>  -> Update<'b>
  val (<*>): Update<'a -> 'b>  -> Update<'a>  -> Update<'b>
  val (>>=): Update<'a> -> ('a -> Update<'b>) -> Update<'b>
  val (>=>): ('a -> Update<'b>) -> ('b -> Update<'c>) ->  'a -> Update<'c>
  val sequenceI: length: ('xs -> int)
              -> iter: (('x -> unit) -> 'xs -> unit)
              -> ofArray: (array<'y> -> 'ys)
              -> Optic<'xs, 'x, 'y, 'ys>

[<AutoOpen>]
module Optic =
  val          view: Optic<'s, 'a,         _,   _> ->               's -> 'a
  val          over: Optic<'s, 'a,        'b,  't> -> ('a -> 'b) -> 's -> 't
  val inline    set: Optic<'s,  _,        'b,  't> ->        'b  -> 's -> 't
  val inline remove: Optic<'s,  _, option< _>, 't> ->               's -> 't

  val foldOf: Optic<'s, 'a, _, _> -> ('x -> 'a -> 'x) -> 'x -> 's -> 'x

  val inline choose: ('s -> Optic<'s, 'a, 'b, 't>) -> Optic<'s, 'a, 'b, 't>

  val inline normalize: ('s -> 't) -> Optic<'s, 't, 's, 't>

  val replace: inn: 'a -> out: 'a -> Optic<'a, 'a> when 'a: equality

  val (<^>): Optic<'s, 'a,      'b     , 's>
          -> Optic<'s,      'c,      'd, 't>
          -> Optic<'s, 'a * 'c, 'b * 'd, 't>

  val (<=>): Optic<'s, 'a, 'b, 'u>
          -> Optic<'u, 'a, 'b, 't>
          -> Optic<'s, 'a, 'b, 't>

  val inline some: (option<'a> -> Update<'b>) -> ('b -> 't) -> 'a -> Update<'t>
  val inline none: (option<'a> -> Update<'b>) -> ('b -> 't)       -> Update<'t>

  val (<|>): Optic<'s, option<'a>, 'b, 't>
          -> Optic<'s, option<'a>, 'b, 't>
          -> Optic<'s, option<'a>, 'b, 't>

  val defaults: 's -> Optic<option<'s>, option<'s>> when 's: equality

  val ofPrism': insert:                             ('b -> option<'t>)
             -> prism: Optic<       's,  option<'a>, 'b,          't>
             ->        Optic<option<'s>, option<'a>, 'b,   option<'t>>

  val ofPrism: prism: Optic<       's,  option<'a>, 'b,        't>
            ->        Optic<option<'s>, option<'a>, 'b, option<'t>>

  val ofTotal: remove:           ('s                        -> option<'t>)
            -> insert:                                   ('b       -> 't)
            -> lens: Optic<       's,         'a,         'b,         't>
            ->       Optic<option<'s>, option<'a>, option<'b>, option<'t>>
